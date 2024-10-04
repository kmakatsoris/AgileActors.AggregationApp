using AgileActors.AggregationApp.Types.Context;
using Microsoft.Extensions.Options;
using System.Linq;

namespace AgileActors.AggregationApp.Utils.AppSettingsUtilsService
{
    public class AppSettingsUtilsService
    {
        private readonly IOptions<AppSettings> _appSettings;

        public AppSettingsUtilsService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public ExternalApis GetExternalApisBySourceName(string sourceName)
        {
            var externalApi = _appSettings?.Value?.ExternalApiSettings?.ExternalApiSettingsList
                ?.FirstOrDefault(api => api.SourceName == sourceName);

            if (externalApi == null)
            {
                throw new ArgumentException($"No API configuration found for source: {sourceName}");
            }

            return externalApi;
        }
    }
}