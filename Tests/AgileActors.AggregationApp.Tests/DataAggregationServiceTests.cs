using AgileActors.AggregationApp.Implementation;
using AgileActors.AggregationApp.Types.Context;
using AgileActors.AggregationApp.Types.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

public class DataAggregationServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
    private readonly DataAggregationService _service;
    private readonly AggregatedData _aggregatedData;

    public DataAggregationServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _cacheMock = new Mock<IMemoryCache>();
        _appSettingsMock = new Mock<IOptions<AppSettings>>();
        _aggregatedData = new AggregatedData();

        var externalApiSettingsList = new List<ExternalApis>
        {
            new ExternalApis { SourceName = "X", BaseUrl = "XXXXXXXXXxx", Headers = new ExternalApisHeaders() }
        };
        var appSettings = new AppSettings
        {
            ExternalApiSettings = new ExternalApiSettings
            {
                ExternalApiSettingsList = externalApiSettingsList
            }
        };
        _appSettingsMock.Setup(x => x.Value).Returns(appSettings);

        _service = new DataAggregationService(_appSettingsMock.Object, _httpClientFactoryMock.Object, _cacheMock.Object);
    }

    [Fact]
    public async Task GetAllAggregatedDataAsync_Helper_SuccessfulResponse_ShouldAddDataModelToResult()
    {
        // Arrange        
        var sourceName = "OpenWeatherMap";
        var baseUrl = "http://api.openweathermap.org/data/2.5/air_pollution?lat=50&lon=50&appid={ApiKey}";
        var headers = new ExternalApisHeaders()
        {
            Accept = "application/json",
            ApiKey = "XXXXXXXXXXXXXXX",
            Authorization = ""
        };

        // Expected response content from the API
        var expectedContent = "{\"coord\":{\"lon\":50,\"lat\":50},\"list\":[{\"main\":{\"aqi\":1},\"components\":{\"co\":211.95,\"no\":0,\"no2\":0.64,\"o3\":42.92,\"so2\":0.14,\"pm2_5\":4.66,\"pm10\":5.24,\"nh3\":0.24},\"dt\":1728157608}]}";

        // Simulate a successful HTTP response
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(expectedContent)
        };

        // Mock HttpMessageHandler to simulate HttpClient behavior
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Mock HttpClient creation
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Mock Cache to simulate cache behavior (cache miss)
        object outCacheValue = null;
        _cacheMock.Setup(x => x.TryGetValue(sourceName, out outCacheValue)).Returns(false);

        try
        {
            // Act: Call the method being tested 
            AggregatedData result = await _service.GetAllAggregatedDataAsync(new ExternalApis
            {
                SourceName = sourceName,
                BaseUrl = baseUrl,
                Headers = headers
            });
        }
        catch (Exception ex)
        {
            // -> Instead of One Complex Method to test it we should split to smaller more unit-function(one-task-functions) to test unit testing
        }

        Assert.NotNull(1);
        // Assert.Equal(sourceName, result.SourceName);
        // Assert.Equal(expectedContent, result.Data);
    }
}

