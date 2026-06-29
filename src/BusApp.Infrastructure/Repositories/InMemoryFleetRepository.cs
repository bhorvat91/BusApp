using BusApp.Application.Interfaces;
using BusApp.Domain.Entities;
using BusApp.Domain.Enums;

namespace BusApp.Infrastructure.Repositories;

public class InMemoryFleetRepository : IFleetRepository
{
    private readonly Company _company = new()
    {
        Id = "firm-1",
        Name = "BusApp Fleet",
        City = "Sarajevo",
        SupportPhone = "+387 61 222 333"
    };

    private readonly List<Bus> _buses =
    [
        new()
        {
            Id = "bus-1",
            Code = "A-12",
            Plate = "K88-J-245",
            Capacity = 52,
            Status = FleetStatus.OnRoute,
            Location = new BusLocation { Lat = 43.8563, Lng = 18.4131, Label = "Stup terminal" },
            AssignedDriverId = "drv-1",
            NextServiceKm = 1400
        },
        new()
        {
            Id = "bus-2",
            Code = "B-07",
            Plate = "M34-T-188",
            Capacity = 48,
            Status = FleetStatus.Available,
            Location = new BusLocation { Lat = 43.8606, Lng = 18.4214, Label = "Depot Ilidza" },
            AssignedDriverId = "drv-2",
            NextServiceKm = 4200
        },
        new()
        {
            Id = "bus-3",
            Code = "C-03",
            Plate = "A11-E-099",
            Capacity = 58,
            Status = FleetStatus.Service,
            Location = new BusLocation { Lat = 43.8478, Lng = 18.3564, Label = "Service centar" },
            AssignedDriverId = "drv-3",
            NextServiceKm = 120
        }
    ];

    private readonly List<Driver> _drivers =
    [
        new()
        {
            Id = "drv-1",
            Name = "Emir Hadzic",
            Phone = "+387 62 100 201",
            Status = FleetStatus.OnRoute,
            CurrentBusId = "bus-1",
            Shift = "06:00 - 14:00",
            Licences = ["D", "CPC"]
        },
        new()
        {
            Id = "drv-2",
            Name = "Lejla Kovac",
            Phone = "+387 62 100 202",
            Status = FleetStatus.Available,
            CurrentBusId = "bus-2",
            Shift = "08:00 - 16:00",
            Licences = ["D", "CPC", "First aid"]
        },
        new()
        {
            Id = "drv-3",
            Name = "Tarik Music",
            Phone = "+387 62 100 203",
            Status = FleetStatus.Service,
            CurrentBusId = "bus-3",
            Shift = "07:00 - 15:00",
            Licences = ["D"]
        }
    ];

    private readonly List<Reservation> _reservations =
    [
        new()
        {
            Id = "res-1",
            Title = "Aerodrom transfer",
            Customer = "Hotel Hills",
            Route = "Sarajevo Airport → Old Town",
            Start = new DateTimeOffset(2026, 6, 26, 8, 0, 0, TimeSpan.FromHours(2)),
            End = new DateTimeOffset(2026, 6, 26, 9, 15, 0, TimeSpan.FromHours(2)),
            BusId = "bus-1",
            DriverId = "drv-1",
            Status = ReservationStatus.Active,
            Pickup = "Sarajevo Airport",
            Dropoff = "Bascarsija"
        },
        new()
        {
            Id = "res-2",
            Title = "Corporate shuttle",
            Customer = "Tech Park",
            Route = "Ilidza → Marijin Dvor",
            Start = new DateTimeOffset(2026, 6, 26, 10, 30, 0, TimeSpan.FromHours(2)),
            End = new DateTimeOffset(2026, 6, 26, 11, 20, 0, TimeSpan.FromHours(2)),
            BusId = "bus-2",
            DriverId = "drv-2",
            Status = ReservationStatus.Planned,
            Pickup = "Ilidza",
            Dropoff = "Marijin Dvor"
        },
        new()
        {
            Id = "res-3",
            Title = "Tourist day trip",
            Customer = "Visit Bosnia",
            Route = "Sarajevo → Mostar",
            Start = new DateTimeOffset(2026, 6, 27, 7, 0, 0, TimeSpan.FromHours(2)),
            End = new DateTimeOffset(2026, 6, 27, 20, 0, 0, TimeSpan.FromHours(2)),
            BusId = "bus-1",
            DriverId = "drv-1",
            Status = ReservationStatus.Planned,
            Pickup = "Hotel Europe",
            Dropoff = "Mostar bus station"
        },
        new()
        {
            Id = "res-4",
            Title = "School transfer",
            Customer = "Osnovna skola Centar",
            Route = "Centar → Vogosca",
            Start = new DateTimeOffset(2026, 6, 28, 7, 30, 0, TimeSpan.FromHours(2)),
            End = new DateTimeOffset(2026, 6, 28, 8, 20, 0, TimeSpan.FromHours(2)),
            BusId = "bus-2",
            DriverId = "drv-2",
            Status = ReservationStatus.Planned,
            Pickup = "Centar",
            Dropoff = "Vogosca"
        }
    ];

    private readonly List<FleetNotification> _notifications =
    [
        new()
        {
            Id = "notif-1",
            Level = NotificationLevel.Warning,
            Title = "Bus C-03 ulazi u servis",
            Description = "Potrebno potvrditi zamjensko vozilo za popodnevnu smjenu."
        },
        new()
        {
            Id = "notif-2",
            Level = NotificationLevel.Info,
            Title = "Nova rezervacija potvrdena",
            Description = "Corporate shuttle za Tech Park dodijeljen vozacu Lejla Kovac."
        },
        new()
        {
            Id = "notif-3",
            Level = NotificationLevel.Critical,
            Title = "Detektovan konflikt rasporeda",
            Description = "Provjeriti preklapanje za Emir Hadzic na 27.06."
        }
    ];

    public Company GetCompany() => _company;
    public IReadOnlyList<Bus> GetBuses() => _buses;
    public Bus? GetBusById(string id) => _buses.Find(b => b.Id == id);
    public IReadOnlyList<Driver> GetDrivers() => _drivers;
    public Driver? GetDriverById(string id) => _drivers.Find(d => d.Id == id);
    public IReadOnlyList<Reservation> GetReservations() => _reservations;
    public IReadOnlyList<FleetNotification> GetNotifications() => _notifications;
}
