using Microsoft.Extensions.Options;
using WeatherMonitor.Api.Data;
using WeatherMonitor.Api.Models;

namespace WeatherMonitor.Api.Services
{
    public class WeatherUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<string> _cities;
        public WeatherUpdateService(IServiceProvider serviceProvider, IOptions<WeatherCityOptions> options)
        {
            _serviceProvider = serviceProvider;
            _cities = options.Value.WeatherCities;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
                var repository = scope.ServiceProvider.GetRequiredService<IWeatherRepository>();

                foreach (var city in _cities)
                {
                    var data = await weatherService.GetWeatherAsync(city);
                    if (data is null) continue;

                    var now = DateTime.UtcNow;
                    data.LastUpdated = now;

                    await repository.UpdateWeatherAsync(data, stoppingToken);

                    await repository.AddHistoryAsync(new WeatherHistory
                    {
                        City = data.City,
                        Country = data.Country,
                        Temperature = data.Temperature,
                        Timestamp = now
                    }, stoppingToken);

                    Console.WriteLine($"[UPDATE] {data.City}, {data.Country}: {data.Temperature}°C @ {now:u}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
