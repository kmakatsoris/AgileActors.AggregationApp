using AgileActors.AggregationApp.Types.Context;
using Microsoft.Extensions.Options;

namespace AgileActors.AggregationApp.Utils.AppSettingsUtilsService
{
    public static class AppSettingsUtilsService
    {
        public static ExternalApis GetExternalApisBySourceName(this IOptions<AppSettings> appSettings, string sourceName)
        {
            var externalApi = appSettings?.Value?.ExternalApiSettings?.ExternalApiSettingsList
                ?.FirstOrDefault(api => api.SourceName == sourceName);

            if (externalApi == null)
            {
                throw new ArgumentException($"No API configuration found for source: {sourceName}");
            }

            return externalApi;
        }
    }
}
