using Microsoft.EntityFrameworkCore;
using WeatherMonitor.Api.Models;

namespace WeatherMonitor.Api.Data
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
        : base(options)
            {
            }
        public DbSet<WeatherData> WeatherEntries { get; set; } = null!;
        public DbSet<WeatherHistory> WeatherHistory { get; set; } = null!;
    }
}
