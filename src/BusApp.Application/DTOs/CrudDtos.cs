using BusApp.Domain.Enums;

namespace BusApp.Application.DTOs;

public record CreateBusRequest(
    string Code,
    string Plate,
    int Capacity,
    FleetStatus Status,
    double LocationLat,
    double LocationLng,
    string LocationLabel,
    string AssignedDriverId,
    int NextServiceKm);

public record UpdateBusRequest(
    string Code,
    string Plate,
    int Capacity,
    FleetStatus Status,
    double LocationLat,
    double LocationLng,
    string LocationLabel,
    string AssignedDriverId,
    int NextServiceKm);

public record CreateDriverRequest(
    string Name,
    string Phone,
    FleetStatus Status,
    string CurrentBusId,
    string Shift,
    List<string> Licences);

public record UpdateDriverRequest(
    string Name,
    string Phone,
    FleetStatus Status,
    string CurrentBusId,
    string Shift,
    List<string> Licences);

public record CreateReservationRequest(
    string Title,
    string Customer,
    string Route,
    DateTimeOffset Start,
    DateTimeOffset End,
    string BusId,
    string DriverId,
    ReservationStatus Status,
    string Pickup,
    string Dropoff);

public record UpdateReservationRequest(
    string Title,
    string Customer,
    string Route,
    DateTimeOffset Start,
    DateTimeOffset End,
    string BusId,
    string DriverId,
    ReservationStatus Status,
    string Pickup,
    string Dropoff);
