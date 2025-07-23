using System.ComponentModel.DataAnnotations;

namespace WeatherMonitor.Api.Models
{
    public class WeatherData
    {
        public int Id { get; set; }

        [Required]
        public string Country { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
