using BusApp.Application.DTOs;
using BusApp.Application.Interfaces;
using BusApp.Domain.Enums;

namespace BusApp.Application.Services;

public class FleetService
{
    private readonly IFleetRepository _repository;

    private static readonly Dictionary<FleetStatus, string> StatusLabels = new()
    {
        { FleetStatus.OnRoute, "Na ruti" },
        { FleetStatus.Available, "Slobodan" },
        { FleetStatus.Break, "Pauza" },
        { FleetStatus.Service, "Servis" },
        { FleetStatus.Offline, "Van mreze" }
    };

    public FleetService(IFleetRepository repository)
    {
        _repository = repository;
    }

    public OverviewDto GetOverview()
    {
        var company = _repository.GetCompany();
        var buses = _repository.GetBuses();
        var drivers = _repository.GetDrivers();
        var reservations = _repository.GetReservations();
        var notifications = _repository.GetNotifications();

        var summary = new DashboardSummaryDto(
            ActiveReservations: reservations.Count(r => r.Status == ReservationStatus.Active),
            PlannedReservations: reservations.Count(r => r.Status == ReservationStatus.Planned),
            ActiveBuses: buses.Count(b => b.Status == FleetStatus.OnRoute),
            AvailableDrivers: drivers.Count(d => d.Status == FleetStatus.Available));

        return new OverviewDto(
            Company: new CompanyDto(company.Id, company.Name, company.City, company.SupportPhone),
            Summary: summary,
            Buses: buses.Select(MapBus).ToList(),
            Drivers: drivers.Select(MapDriver).ToList(),
            Notifications: notifications.Select(n => new NotificationDto(
                n.Id, n.Level.ToString().ToLowerInvariant(), n.Title, n.Description)).ToList());
    }

    public CalendarDto GetCalendar()
    {
        var reservations = _repository.GetReservations();
        var dtos = reservations.Select(MapReservation).ToList();

        var days = dtos
            .GroupBy(r => r.Start.ToString("yyyy-MM-dd"))
            .ToDictionary(g => g.Key, g => g.ToList());

        return new CalendarDto(days, dtos);
    }

    public List<ReservationDetailDto> GetReservations()
    {
        var reservations = _repository.GetReservations();

        return reservations.Select(r =>
        {
            var bus = _repository.GetBusById(r.BusId);
            var driver = _repository.GetDriverById(r.DriverId);

            return new ReservationDetailDto(
                r.Id, r.Title, r.Customer, r.Route,
                r.Start, r.End, r.BusId, r.DriverId,
                r.Status.ToString().ToLowerInvariant(),
                r.Pickup, r.Dropoff,
                bus is not null ? MapBus(bus) : null,
                driver is not null ? MapDriver(driver) : null);
        }).ToList();
    }

    public DriverDetailDto? GetDriverDetail(string driverId)
    {
        var driver = _repository.GetDriverById(driverId);
        if (driver is null) return null;

        var bus = _repository.GetBusById(driver.CurrentBusId);
        var reservations = _repository.GetReservations()
            .Where(r => r.DriverId == driver.Id)
            .Select(MapReservation)
            .ToList();

        return new DriverDetailDto(
            driver.Id, driver.Name, driver.Phone,
            MapStatus(driver.Status),
            StatusLabels.GetValueOrDefault(driver.Status, ""),
            driver.CurrentBusId, driver.Shift, driver.Licences,
            bus is not null ? MapBus(bus) : null,
            reservations);
    }

    private static BusDto MapBus(Domain.Entities.Bus bus) => new(
        bus.Id, bus.Code, bus.Plate, bus.Capacity,
        MapStatus(bus.Status),
        new BusLocationDto(bus.Location.Lat, bus.Location.Lng, bus.Location.Label),
        bus.AssignedDriverId, bus.NextServiceKm);

    private static DriverDto MapDriver(Domain.Entities.Driver driver) => new(
        driver.Id, driver.Name, driver.Phone,
        MapStatus(driver.Status),
        driver.CurrentBusId, driver.Shift, driver.Licences);

    private static ReservationDto MapReservation(Domain.Entities.Reservation r) => new(
        r.Id, r.Title, r.Customer, r.Route,
        r.Start, r.End, r.BusId, r.DriverId,
        r.Status.ToString().ToLowerInvariant(),
        r.Pickup, r.Dropoff);

    private static string MapStatus(FleetStatus status) => status switch
    {
        FleetStatus.OnRoute => "on-route",
        FleetStatus.Available => "available",
        FleetStatus.Break => "break",
        FleetStatus.Service => "service",
        FleetStatus.Offline => "offline",
        _ => "offline"
    };
}
