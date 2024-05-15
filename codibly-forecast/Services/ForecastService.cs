using System.Globalization;
using System.Text.Json;
using codibly_forecast.Exceptions;
using codibly_forecast.Models;

namespace codibly_forecast.Services;

public class ForecastService : IForecastService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _options;

    public ForecastService(HttpClient httpClient, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClient;
        _httpClientFactory = httpClientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public double CalculateEnergy(double sunshineDuration)
    {
        var panelPower = 2.5;
        var efficiency = 0.2;
        return panelPower * (sunshineDuration / 3600) * efficiency;
    }

    public async Task<List<EnergyResultDto>> GetWeatherDataAsync(double latitude, double longitude)
    {
        if (latitude == null || longitude == null)
        {
            throw new BadRequestException("Latitude or longitude cannot be null.");
        }

        if (!(latitude is double) || !(longitude is double))
        {
            throw new BadRequestException("Latitude and longitude must be of type double.");
        }

        if (latitude < -90 || latitude > 90)
        {
            throw new BadRequestException("Latitude must be between -90 and 90 degrees.");
        }

        if (longitude < -180 || longitude > 180)
        {
            throw new BadRequestException("Longitude must be between -180 and 180 degrees.");
        }

        string apiUrl =
            $"https://api.open-meteo.com/v1/forecast?latitude={latitude.ToString(CultureInfo.InvariantCulture)}&longitude={longitude.ToString(CultureInfo.InvariantCulture)}&daily=weather_code,sunshine_duration,temperature_2m_min,temperature_2m_max";

        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            List<EnergyResultDto> energyResults = new List<EnergyResultDto>();
            var jsonContent = await response.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<ForecastResultDto>(jsonContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            for (var i = 0; i < 7; i++)
             {
                energyResults.Add(new EnergyResultDto()
                    {
                        Date = weatherData.daily.time[i],
                        WeatherCode = weatherData.daily.weather_code[i],
                        MinTemperature = weatherData.daily.temperature_2m_min[i],
                        MaxTemperature = weatherData.daily.temperature_2m_max[i],
                        EstimatedGeneratedEnergy = CalculateEnergy(weatherData.daily.sunshine_duration[i]),
                        EnergyUnits = new EnergyResultUnitsDto()
                        {
                            Date = weatherData.daily_units.time,
                            WeatherCode = weatherData.daily_units.weather_code,
                            MinTemperature = weatherData.daily_units.temperature_2m_min,
                            MaxTemperature = weatherData.daily_units.temperature_2m_max
                        }
                    }
                );
            }

            return energyResults;
        }
        else
        {
            throw new Exception($"Failed to fetch weather");
        }
    }
}