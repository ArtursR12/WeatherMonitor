using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherMonitor.Api.Models;

namespace WeatherMonitor.Api.Services
{
    public class OpenWeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<OpenWeatherService> _logger;

        public OpenWeatherService(HttpClient httpClient, IOptions<OpenWeatherConfig> config, ILogger<OpenWeatherService> logger)
        {
            _httpClient = httpClient;
            _apiKey = config.Value.ApiKey;
            _logger = logger;
        }

        public async Task<WeatherData?> GetWeatherAsync(string city)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var cityName = root.GetProperty("name").GetString();
                var country = root.GetProperty("sys").GetProperty("country").GetString();
                var temp = root.GetProperty("main").GetProperty("temp").GetDouble();

                return new WeatherData
                {
                    City = cityName ?? city,
                    Country = country ?? "??",
                    Temperature = temp,
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch weather data for city {City}", city);
                return null;
            }
        }
    }
}
