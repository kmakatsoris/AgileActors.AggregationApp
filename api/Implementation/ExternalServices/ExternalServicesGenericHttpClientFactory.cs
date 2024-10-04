using Microsoft.Extensions.Options;
using AgileActors.AggregationApp.Types.Context;
using AgileActors.AggregationApp.Utils.AppSettingsUtilsService;

namespace AgileActors.AggregationApp.Implementation.ExternalServices
{
    public class ExternalServicesGenericHttpClientFactory<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettingsUtilsService _appSettingsUtilsService;
        private readonly ExternalApis? ExternalApisConfig;

        public ExternalServicesGenericHttpClientFactory(IHttpClientFactory httpClientFactory, AppSettingsUtilsService appSettingsUtilsService, string sourceName)
        {
            _httpClientFactory = httpClientFactory;
            _appSettingsUtilsService = appSettingsUtilsService;
            ExternalApisConfig = _appSettingsUtilsService?.GetExternalApisBySourceName(sourceName) ?? null;
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
