

using AgileActors.AggregationApp.Types.Context;
using AgileActors.AggregationApp.Types.DTOs;

namespace AgileActors.AggregationApp.Interfaces
{
    public interface IDataAggregationService
    {
        Task<AggregatedData> GetAllAggregatedDataAsync(ExternalApis? externalApis = null);
        Task<AggregatedData> GetAllAggregatedData_Filtered_Async(string sourceName, string author, DateTime? publishedAfter);
    }
}