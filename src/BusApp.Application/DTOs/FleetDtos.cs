using BusApp.Domain.Enums;

namespace BusApp.Application.DTOs;

public record DashboardSummaryDto(
    int ActiveReservations,
    int PlannedReservations,
    int ActiveBuses,
    int AvailableDrivers);

public record BusLocationDto(double Lat, double Lng, string Label);

public record BusDto(
    string Id,
    string Code,
    string Plate,
    int Capacity,
    string Status,
    BusLocationDto Location,
    string AssignedDriverId,
    int NextServiceKm);

public record DriverDto(
    string Id,
    string Name,
    string Phone,
    string Status,
    string CurrentBusId,
    string Shift,
    List<string> Licences);

public record ReservationDto(
    string Id,
    string Title,
    string Customer,
    string Route,
    DateTimeOffset Start,
    DateTimeOffset End,
    string BusId,
    string DriverId,
    string Status,
    string Pickup,
    string Dropoff);

public record NotificationDto(
    string Id,
    string Level,
    string Title,
    string Description);

public record OverviewDto(
    CompanyDto Company,
    DashboardSummaryDto Summary,
    List<BusDto> Buses,
    List<DriverDto> Drivers,
    List<NotificationDto> Notifications);

public record CompanyDto(
    string Id,
    string Name,
    string City,
    string SupportPhone);

public record CalendarDto(
    Dictionary<string, List<ReservationDto>> Days,
    List<ReservationDto> Reservations);

public record ReservationDetailDto(
    string Id,
    string Title,
    string Customer,
    string Route,
    DateTimeOffset Start,
    DateTimeOffset End,
    string BusId,
    string DriverId,
    string Status,
    string Pickup,
    string Dropoff,
    BusDto? Bus,
    DriverDto? Driver);

public record DriverDetailDto(
    string Id,
    string Name,
    string Phone,
    string Status,
    string StatusLabel,
    string CurrentBusId,
    string Shift,
    List<string> Licences,
    BusDto? Bus,
    List<ReservationDto> Schedule);
