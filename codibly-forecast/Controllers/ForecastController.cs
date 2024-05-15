using codibly_forecast.Models;
using codibly_forecast.Services;
using Microsoft.AspNetCore.Mvc;

[Route("/api/forecast")]
[ApiController]
public class ForecastController : ControllerBase
{
    private readonly IForecastService _forecastService;

    public ForecastController(IForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EnergyResultDto>>> GetForecast([FromQuery] double latitude, double longitude)
    {
        var result = await _forecastService.GetWeatherDataAsync(latitude, longitude);
        return Ok(result);
    }
}