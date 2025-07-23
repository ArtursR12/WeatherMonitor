using WeatherMonitor.Api.Models;

namespace WeatherMonitor.Api.Data
{
    public interface IWeatherRepository
    {
        Task UpdateWeatherAsync(WeatherData data, CancellationToken token);
        Task AddHistoryAsync(WeatherHistory history, CancellationToken token);
    }
}
