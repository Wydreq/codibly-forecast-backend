namespace codibly_forecast.Models;

public class EnergyResultDto
{
    public string Date { get; set; }
    public double WeatherCode { get; set; }
    public double MinTemperature { get; set; }
    public double MaxTemperature { get; set; }
    public double EstimatedGeneratedEnergy { get; set; }
    public EnergyResultUnitsDto EnergyUnits { get; set; }
}
public class EnergyResultUnitsDto
{
    public string Date { get; set; }
    public string WeatherCode { get; set; }
    public string MinTemperature { get; set; }
    public string MaxTemperature { get; set; }
}