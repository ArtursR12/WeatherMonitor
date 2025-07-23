using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WeatherMonitor.Api.Data;
using WeatherMonitor.Api.Models;

namespace WeatherMonitor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherDbContext _db;

        public WeatherController(WeatherDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherData>> Get()
        {
            return await _db.WeatherEntries
                .OrderBy(w => w.Country)
                .ThenBy(w => w.City)
                .ToListAsync();
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] string city)
        {
            var history = await _db.WeatherHistory
                .Where(x => x.City == city)
                .OrderBy(x => x.Timestamp)
                .ToListAsync();

            return Ok(history);
        }

        [HttpGet("minmax")]
        public async Task<IActionResult> GetMinMax()
        {
            var minMaxData = await _db.WeatherHistory
                .GroupBy(x => new { x.Country, x.City })
                .Select(g => new
                {
                    g.Key.Country,
                    g.Key.City,
                    MinTemperature = g.Min(x => x.Temperature),
                    MaxTemperature = g.Max(x => x.Temperature),
                    LastUpdated = g.Max(x => x.Timestamp)
                })
                .ToListAsync();

            return Ok(minMaxData);
        }

        [HttpGet("cities")]
        public IActionResult GetCities([FromServices] IOptions<WeatherCityOptions> options)
        {
            return Ok(options.Value.WeatherCities);
        }
    }
}
