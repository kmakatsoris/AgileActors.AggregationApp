using AgileActors.AggregationApp.Exceptions;
using AgileActors.AggregationApp.Interfaces;
using AgileActors.AggregationApp.Types.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AgileActors.AggregationApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataAggregationController : ControllerBase
    {
        private readonly IDataAggregationService _dataAggregationService;

        public DataAggregationController(IDataAggregationService dataAggregationService)
        {
            _dataAggregationService = dataAggregationService;
        }

        [HttpPost("listAll")] // We can perform paging and other optimization techniques,...
        public async Task<AggregatedData> GetAllAggregatedDataAsync()
        {
            return await DefaultException.ExceptionControllerHandler(async () =>
            {
                return await _dataAggregationService?.GetAllAggregatedDataAsync();
            });
        }

        [HttpPost("listFilteredAndSorted")]
        public async Task<AggregatedData> GetFilteredAggregatedDataAsync([FromQuery] string sourceName, [FromQuery] string author, [FromQuery] DateTime publishedAfter)
        {
            return await DefaultException.ExceptionControllerHandler(async () =>
            {
                return await _dataAggregationService?.GetAllAggregatedData_Filtered_Async(sourceName, author, publishedAfter);
            });
        }
    }
}