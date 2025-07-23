using Microsoft.EntityFrameworkCore;
using WeatherMonitor.Api.Models;

namespace WeatherMonitor.Api.Data
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly WeatherDbContext _db;

        public WeatherRepository(WeatherDbContext db)
        {
            _db = db;
        }

        public async Task UpdateWeatherAsync(WeatherData data, CancellationToken token)
        {
            var existing = await _db.WeatherEntries
                .FirstOrDefaultAsync(x => x.City == data.City && x.Country == data.Country, token);

            if (existing != null)
            {
                existing.Temperature = data.Temperature;
                existing.LastUpdated = data.LastUpdated;
            }
            else
            {
                _db.WeatherEntries.Add(data);
            }

            await _db.SaveChangesAsync(token);
        }

        public async Task AddHistoryAsync(WeatherHistory history, CancellationToken token)
        {
            _db.WeatherHistory.Add(history);
            await _db.SaveChangesAsync(token);
        }
    }
}
