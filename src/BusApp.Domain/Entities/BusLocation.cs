namespace BusApp.Domain.Entities;

public class BusLocation
{
    public double Lat { get; set; }
    public double Lng { get; set; }
    public string Label { get; set; } = string.Empty;
}
