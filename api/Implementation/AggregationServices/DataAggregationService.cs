using AgileActors.AggregationApp.Interfaces;
using AgileActors.AggregationApp.Types.Context;
using AgileActors.AggregationApp.Types.DTOs;
using AgileActors.AggregationApp.Types.Models.NewsData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AgileActors.AggregationApp.Implementation
{
    public class DataAggregationService : IDataAggregationService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;

        public DataAggregationService(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _appSettings = appSettings;
            _httpClientFactory = httpClientFactory;
            _cache = memoryCache;
        }

        #region Public Methods
        public async Task<AggregatedData> GetAllAggregatedDataAsync(ExternalApis? externalApis = null)
        {
            AggregatedData result = new AggregatedData();
            List<ExternalApis>? listOfExternalApis = _appSettings?.Value?.ExternalApiSettings?.ExternalApiSettingsList;

            if (listOfExternalApis == null || listOfExternalApis.Count == 0 || listOfExternalApis.Any(l => l == null))
                return result;

            var tasks = listOfExternalApis.Select(async api =>
            {

                ExternalApis extApi = InitParams(externalApis, api);

                if (_cache.TryGetValue(extApi.SourceName, out DataModel cachedData))
                {
                    lock (result)
                    {
                        result.Data.Add(cachedData);
                    }
                    return;
                }

                await GetAllAggregatedDataAsync_Helper(result, extApi.Headers, extApi.BaseUrl, extApi.SourceName);
            });

            await Task.WhenAll(tasks);

            return result;
        }

        public async Task<AggregatedData> GetAllAggregatedData_Filtered_Async(string sourceName, string author, DateTime? publishedAfter)
        {
            if (string.IsNullOrEmpty(sourceName)) sourceName = "News";
            if (string.IsNullOrEmpty(author)) author = "Karla";
            if (publishedAfter == null) publishedAfter = DateTime.Now.AddDays(-7);

            AggregatedData result = await GetAllAggregatedDataAsync();
            try
            {
                // ***************************************************** ***************************************************** ***************************************************** 
                // Because we get response from different sources and not all of them can be usefull, for educational purposes, to be filtered -> We will perform filtering on the News data [#Education_Reasons :)].
                // ***************************************************** ***************************************************** ***************************************************** 
                var tmpData = result?.Data?.FirstOrDefault(d => d?.SourceName?.Equals(sourceName) == true)?.Data;
                if (string.IsNullOrEmpty(tmpData)) return result;
                var newsData = JsonConvert.DeserializeObject<NewsData>(tmpData);
                var filteredArticles = FilterArticles(newsData?.Articles, author, publishedAfter);
                AggregatedData newResult = result;
                foreach (var r in newResult?.Data)
                {
                    if (r != null && r?.SourceName?.Equals(sourceName) == true)
                    {
                        r.Data = JsonConvert.SerializeObject(filteredArticles);
                    }
                }
                return newResult;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Exception($"Unexpected error during filtering procedure: {ex?.Message}"));
            }
            return result;
        }
        #endregion

        #region Private Methods
        private ExternalApis InitParams(ExternalApis? externalApis, ExternalApis api)
        {
            ExternalApis result = new ExternalApis();
            if (externalApis != null)
            {
                result.SourceName = externalApis?.SourceName ?? "";
                result.BaseUrl = externalApis?.BaseUrl ?? "";
                result.Headers = externalApis?.Headers ?? new ExternalApisHeaders();
            }
            else
            {
                result.SourceName = api.SourceName;
                result.BaseUrl = api.BaseUrl;
                result.Headers = api.Headers;
            }
            return result;
        }
        private async Task GetAllAggregatedDataAsync_Helper(AggregatedData result, ExternalApisHeaders headers, string baseUrl, string sourceName)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                try
                {
                    AdjustHeaders(headers, httpClient);
                    baseUrl = AdjustBaseUrl(headers, baseUrl);
                    var response = await httpClient.GetAsync(baseUrl);
                    response.EnsureSuccessStatusCode();

                    string? curData = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(curData))
                    {
                        var dataModel = new DataModel
                        {
                            SourceName = sourceName,
                            Data = curData
                        };

                        _cache.Set(sourceName, dataModel, TimeSpan.FromMinutes(10));

                        lock (result)
                        {
                            result.Data.Add(dataModel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lock (result)
                    {
                        result.Errors.Add(new Exception($"Unexpected error from {sourceName}: {ex?.Message}"));
                    }
                }
            }
        }
        private void AdjustHeaders(ExternalApisHeaders headers, HttpClient? httpClient)
        {
            if (httpClient == null || headers == null) return;
            if (!string.IsNullOrEmpty(headers?.Authorization)) httpClient.DefaultRequestHeaders.Add("Authorization", headers?.Authorization);
            if (!string.IsNullOrEmpty(headers?.Accept)) httpClient.DefaultRequestHeaders.Add("Accept", headers?.Accept);
        }

        private string AdjustBaseUrl(ExternalApisHeaders headers, string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl) || headers == null) return "";
            return baseUrl.Replace("{ApiKey}", headers?.ApiKey);
        }

        public static List<Article> FilterArticles(List<Article> articles, string author, DateTime? publishedAfter = null)
        {
            if (articles == null || articles.Count == 0) return new List<Article>();

            return articles
                .Where(article =>
                    (author == null || (article?.Author != null && article.Author.Contains(author, StringComparison.OrdinalIgnoreCase))) &&
                    (!publishedAfter.HasValue || article.PublishedAt >= publishedAfter.Value))
                .ToList();
        }

        #endregion
    }
}