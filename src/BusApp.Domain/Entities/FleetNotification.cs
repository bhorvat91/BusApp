using BusApp.Domain.Enums;

namespace BusApp.Domain.Entities;

public class FleetNotification
{
    public string Id { get; set; } = string.Empty;
    public NotificationLevel Level { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
