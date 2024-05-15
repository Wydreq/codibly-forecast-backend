// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Daily
{
    public List<string> time { get; set; }
    public List<int> weather_code { get; set; }
    public List<double> sunshine_duration { get; set; }
    public List<double> temperature_2m_min { get; set; }
    public List<double> temperature_2m_max { get; set; }
}

public class DailyUnits
{
    public string time { get; set; }
    public string weather_code { get; set; }
    public string sunshine_duration { get; set; }
    public string temperature_2m_min { get; set; }
    public string temperature_2m_max { get; set; }
}

public class ForecastResultDto
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public double generationtime_ms { get; set; }
    public int utc_offset_seconds { get; set; }
    public string timezone { get; set; }
    public string timezone_abbreviation { get; set; }
    public double elevation { get; set; }
    public DailyUnits daily_units { get; set; }
    public Daily daily { get; set; }
}