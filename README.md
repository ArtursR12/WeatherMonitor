# WeatherMonitor

WeatherMonitor is a .NET Core API that retrieves weather data from OpenWeather and stores it in a local database.  
It supports:

- Scheduled weather updates
- Historical tracking of weather changes
- Modular service architecture
- Unit tests with xUnit and Moq

## Getting Started

1. Clone the repository  
   `git clone https://github.com/ArtursR12/WeatherMonitor.git`

2. Run the API  
   `dotnet run --project WeatherMonitor.Api`

3. Run tests  
   `dotnet test WeatherMonitor.Tests`

## Technologies

- ASP.NET Core
- Entity Framework Core
- SQLite
- xUnit, Moq
- OpenWeather API
- Chart.js
