using System;

namespace Api70.Core.Domain;

public record WeatherForecast(DateTime Date, int TemperatureC, string Summary)
{
    public DateTime Date { get; } = Date;

    public int TemperatureC { get; } = TemperatureC;

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string Summary { get; } = Summary;
}
