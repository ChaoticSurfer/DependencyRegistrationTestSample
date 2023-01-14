namespace WebApplication1.Services;

public interface IWeatherService
{
    public IEnumerable<WeatherForecast> GetWeeklyForcast();
}