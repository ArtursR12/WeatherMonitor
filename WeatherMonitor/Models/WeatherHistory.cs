namespace WeatherMonitor.Api.Models
{
    public class WeatherHistory
    {
        public int Id { get; set; }
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public double Temperature { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
