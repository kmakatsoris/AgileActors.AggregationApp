using AgileActors.AggregationApp.Types.Context;
using AgileActors.AggregationApp.Types.DTOs;
using Microsoft.Extensions.Options;

namespace AgileActors.AggregationApp.Implementation.AggregationServices
{
    public class DataAggregationService
    {
        private readonly IOptions<AppSettings> _appSettings;

        public DataAggregationService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<AggregatedData> GetAllAggregatedDataAsync()
        {
            AggregatedData result = new AggregatedData();
            List<ExternalApis>? listOfExternalApis = _appSettings?.Value?.ExternalApiSettings?.ExternalApiSettingsList;
            if (listOfExternalApis == null || listOfExternalApis?.Count() > 0 || listOfExternalApis?.Any(l => l == null) == true) return result;
            foreach (var api in listOfExternalApis)
            {
                var sourceName = api.SourceName;
                var baseUrl = api.BaseUrl;
                var apiKey = api.ApiKey;

                // Make an API call based on the configuration

                // Aggregate the data as needed                
            }

            // Return aggregated data
            return result;
        }
    }

}