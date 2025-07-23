using Microsoft.EntityFrameworkCore;
using WeatherMonitor.Api.Services;
using WeatherMonitor.Api.Data;
using WeatherMonitor.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OpenWeatherConfig>(
builder.Configuration.GetSection("OpenWeather"));
builder.Services.Configure<WeatherCityOptions>(
builder.Configuration.GetSection("WeatherCityOptions"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlite("Data Source=weather.db"));

builder.Services.AddHttpClient<IWeatherService, OpenWeatherService>();
builder.Services.AddHostedService<WeatherUpdateService>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

var app = builder.Build();

app.UseDefaultFiles();     
app.UseStaticFiles();       

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
