using Microsoft.Extensions.Options;
using AgileActors.AggregationApp.Types.Context;
using AgileActors.AggregationApp.Utils.AppSettingsUtilsService;

namespace AgileActors.AggregationApp.Implementation.ExternalServices
{
    public class ExternalServicesGenericHttpClientFactory<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ExternalApis? ExternalApisConfig;

        public ExternalServicesGenericHttpClientFactory(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, string sourceName)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings;
            ExternalApisConfig = _appSettings?.GetExternalApisBySourceName(sourceName) ?? null;
        }

        public async Task<T?> GetDataAsync()
        {
            if (string.IsNullOrEmpty(ExternalApisConfig?.BaseUrl)) return null;
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(ExternalApisConfig?.BaseUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
