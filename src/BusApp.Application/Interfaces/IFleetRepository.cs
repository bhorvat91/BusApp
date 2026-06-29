using BusApp.Domain.Entities;

namespace BusApp.Application.Interfaces;

public interface IFleetRepository
{
    Company GetCompany();

    IReadOnlyList<Bus> GetBuses();
    Bus? GetBusById(string id);
    Task<Bus> CreateBusAsync(Bus bus);
    Task<Bus?> UpdateBusAsync(Bus bus);
    Task<bool> DeleteBusAsync(string id);

    IReadOnlyList<Driver> GetDrivers();
    Driver? GetDriverById(string id);
    Task<Driver> CreateDriverAsync(Driver driver);
    Task<Driver?> UpdateDriverAsync(Driver driver);
    Task<bool> DeleteDriverAsync(string id);

    IReadOnlyList<Reservation> GetReservations();
    Reservation? GetReservationById(string id);
    Task<Reservation> CreateReservationAsync(Reservation reservation);
    Task<Reservation?> UpdateReservationAsync(Reservation reservation);
    Task<bool> DeleteReservationAsync(string id);

    IReadOnlyList<FleetNotification> GetNotifications();
}
