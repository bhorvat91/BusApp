using BusApp.Domain.Enums;

namespace BusApp.Domain.Entities;

public class Driver
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public FleetStatus Status { get; set; }
    public string CurrentBusId { get; set; } = string.Empty;
    public string Shift { get; set; } = string.Empty;
    public List<string> Licences { get; set; } = [];
}
