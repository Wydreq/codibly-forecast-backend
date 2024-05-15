using AutoMapper;
using codibly_forecast.Models;

namespace codibly_forecast;

public class ForecastMappingProfile : Profile
{
    public ForecastMappingProfile()
    {
        CreateMap<ForecastResultDto, EnergyResultDto>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.daily.time[0])) // Example for the first item, extend as needed
            .ForMember(dest => dest.WeatherCode, opt => opt.MapFrom(src => src.daily.weather_code[0])) // Example for the first item
            .ForMember(dest => dest.MinTemperature, opt => opt.MapFrom(src => src.daily.temperature_2m_min[0])) // Example for the first item
            .ForMember(dest => dest.MaxTemperature, opt => opt.MapFrom(src => src.daily.temperature_2m_max[0])) // Example for the first item
            .ForMember(dest => dest.EstimatedGeneratedEnergy, opt => opt.Ignore()) // Calculated separately
            .ForMember(dest => dest.EnergyUnits, opt => opt.MapFrom(src => new EnergyResultUnitsDto
            {
                Date = src.daily_units.time,
                WeatherCode = src.daily_units.weather_code,
                MinTemperature = src.daily_units.temperature_2m_min,
                MaxTemperature = src.daily_units.temperature_2m_max
            }));
    }
}