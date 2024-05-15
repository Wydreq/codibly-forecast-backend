using codibly_forecast.Models;

namespace codibly_forecast.Services;

public interface IForecastService
{
    Task<List<EnergyResultDto>> GetWeatherDataAsync(double latitude, double longitude);
}