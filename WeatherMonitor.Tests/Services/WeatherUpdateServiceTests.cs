using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using WeatherMonitor.Api.Data;
using WeatherMonitor.Api.Models;
using WeatherMonitor.Api.Services;

namespace WeatherMonitor.Tests.Services;

public class WeatherUpdateServiceTests
{
    [Fact]
    public async Task Should_Call_GetWeather_And_Save_With_Correct_Data()
    {
        // Arrange
        var weatherServiceMock = new Mock<IWeatherService>();
        var repoMock = new Mock<IWeatherRepository>();

        var sampleData = new WeatherData
        {
            City = "Berlin",
            Country = "DE",
            Temperature = 21.5,
            LastUpdated = DateTime.UtcNow
        };

        weatherServiceMock.Setup(s => s.GetWeatherAsync("Berlin"))
                          .ReturnsAsync(sampleData);

        var services = new ServiceCollection()
            .AddSingleton<IWeatherService>(weatherServiceMock.Object)
            .AddSingleton<IWeatherRepository>(repoMock.Object)
            .BuildServiceProvider();

        var options = Options.Create(new WeatherCityOptions
        {
            WeatherCities = new List<string> { "Berlin" }
        });

        var service = new WeatherUpdateService(services, options);
        var token = new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token;

        // Act
        await service.StartAsync(token);

        // Assert
        weatherServiceMock.Verify(x => x.GetWeatherAsync("Berlin"), Times.AtLeastOnce);

        repoMock.Verify(x => x.UpdateWeatherAsync(
            It.Is<WeatherData>(d => d.City == "Berlin" && d.Country == "DE"),
            It.IsAny<CancellationToken>()), Times.AtLeastOnce);

        repoMock.Verify(x => x.AddHistoryAsync(
            It.Is<WeatherHistory>(h => h.City == "Berlin" && h.Temperature == 21.5),
            It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}
