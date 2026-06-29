using BusApp.Domain.Enums;

namespace BusApp.Domain.Entities;

public class Bus
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public FleetStatus Status { get; set; }
    public BusLocation Location { get; set; } = new();
    public string AssignedDriverId { get; set; } = string.Empty;
    public int NextServiceKm { get; set; }
}
