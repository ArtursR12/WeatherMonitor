using WeatherMonitor.Api.Models;

namespace WeatherMonitor.Api.Services
{
    public interface IWeatherService
    {
        Task<WeatherData?> GetWeatherAsync(string city);
    }
}
