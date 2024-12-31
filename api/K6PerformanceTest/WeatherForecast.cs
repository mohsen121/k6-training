﻿using System.ComponentModel.DataAnnotations.Schema;

public class WeatherForecast
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    
    [NotMapped]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class WeatherForecastWithRowNumber : WeatherForecast
{
    public long RowNumber { get; set; }
}