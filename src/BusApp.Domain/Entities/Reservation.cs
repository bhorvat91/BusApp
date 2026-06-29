using BusApp.Domain.Enums;

namespace BusApp.Domain.Entities;

public class Reservation
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public string BusId { get; set; } = string.Empty;
    public string DriverId { get; set; } = string.Empty;
    public ReservationStatus Status { get; set; }
    public string Pickup { get; set; } = string.Empty;
    public string Dropoff { get; set; } = string.Empty;
}
