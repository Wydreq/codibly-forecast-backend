using AutoMapper;
using codibly_forecast.Models;

namespace codibly_forecast;

public class ForecastMappingProfile : Profile
{
    public ForecastMappingProfile()
    {
        CreateMap<EnergyResultDto, ForecastResultDto>();
    }
}