using System.Globalization;
using System.Text.Json;
using AutoMapper;
using codibly_forecast.Exceptions;
using codibly_forecast.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace codibly_forecast.Services;

public class ForecastService : IForecastService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _options;

    public ForecastService(HttpClient httpClient, IHttpClientFactory httpClientFactory, IMapper mapper,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;
        _configuration = configuration;
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

        var query = new Dictionary<string, string>()
        {
            ["latitude"] = latitude.ToString(CultureInfo.InvariantCulture),
            ["longitude"] = longitude.ToString(CultureInfo.InvariantCulture),
            ["daily"] = "weather_code,sunshine_duration,temperature_2m_min,temperature_2m_max"
        };

        var uriBuilder = new UriBuilder(_configuration["baseUrl"]);
        var apiUrl = QueryHelpers.AddQueryString(uriBuilder.ToString(), query);
        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var jsonContent = await response.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<ForecastResultDto>(jsonContent, _options);

            var energyResults = new List<EnergyResultDto>();
            for (var i = 0; i < 7; i++)
            {
                var energyResult = _mapper.Map<EnergyResultDto>(weatherData);
                energyResult.Date = weatherData.daily.time[i];
                energyResult.WeatherCode = weatherData.daily.weather_code[i];
                energyResult.MinTemperature = weatherData.daily.temperature_2m_min[i];
                energyResult.MaxTemperature = weatherData.daily.temperature_2m_max[i];
                energyResult.EstimatedGeneratedEnergy = CalculateEnergy(weatherData.daily.sunshine_duration[i]);

                energyResults.Add(energyResult);
            }

            return energyResults;
        }
        else
        {
            throw new Exception($"Failed to fetch weather");
        }
    }
}