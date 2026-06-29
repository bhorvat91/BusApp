using BusApp.Domain.Entities;

namespace BusApp.Application.Interfaces;

public interface IFleetRepository
{
    Company GetCompany();
    IReadOnlyList<Bus> GetBuses();
    Bus? GetBusById(string id);
    IReadOnlyList<Driver> GetDrivers();
    Driver? GetDriverById(string id);
    IReadOnlyList<Reservation> GetReservations();
    IReadOnlyList<FleetNotification> GetNotifications();
}
