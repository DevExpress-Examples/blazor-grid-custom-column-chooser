using System;
using System.Linq;
using System.Threading.Tasks;

namespace CustomColumnChooser.Services
{
    public class WeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild",
            "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastsAsync(int days = 5)
        {
            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var forecasts = Enumerable.Range(1, days).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

            return Task.FromResult(forecasts);
        }
    }
}