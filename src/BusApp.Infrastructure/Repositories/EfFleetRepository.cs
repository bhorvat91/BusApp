using BusApp.Application.Interfaces;
using BusApp.Domain.Entities;
using BusApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Infrastructure.Repositories;

public class EfFleetRepository : IFleetRepository
{
    private readonly BusAppDbContext _db;

    public EfFleetRepository(BusAppDbContext db)
    {
        _db = db;
    }

    public Company GetCompany() =>
        _db.Companies.First();

    public IReadOnlyList<Bus> GetBuses() =>
        _db.Buses.ToList();

    public Bus? GetBusById(string id) =>
        _db.Buses.FirstOrDefault(b => b.Id == id);

    public async Task<Bus> CreateBusAsync(Bus bus)
    {
        if (string.IsNullOrEmpty(bus.Id))
            bus.Id = $"bus-{Guid.NewGuid():N}"[..12];
        _db.Buses.Add(bus);
        await _db.SaveChangesAsync();
        return bus;
    }

    public async Task<Bus?> UpdateBusAsync(Bus bus)
    {
        var existing = await _db.Buses.FindAsync(bus.Id);
        if (existing is null) return null;
        _db.Entry(existing).CurrentValues.SetValues(bus);
        existing.Location = bus.Location;
        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteBusAsync(string id)
    {
        var bus = await _db.Buses.FindAsync(id);
        if (bus is null) return false;
        _db.Buses.Remove(bus);
        await _db.SaveChangesAsync();
        return true;
    }

    public IReadOnlyList<Driver> GetDrivers() =>
        _db.Drivers.ToList();

    public Driver? GetDriverById(string id) =>
        _db.Drivers.FirstOrDefault(d => d.Id == id);

    public async Task<Driver> CreateDriverAsync(Driver driver)
    {
        if (string.IsNullOrEmpty(driver.Id))
            driver.Id = $"drv-{Guid.NewGuid():N}"[..12];
        _db.Drivers.Add(driver);
        await _db.SaveChangesAsync();
        return driver;
    }

    public async Task<Driver?> UpdateDriverAsync(Driver driver)
    {
        var existing = await _db.Drivers.FindAsync(driver.Id);
        if (existing is null) return null;
        _db.Entry(existing).CurrentValues.SetValues(driver);
        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteDriverAsync(string id)
    {
        var driver = await _db.Drivers.FindAsync(id);
        if (driver is null) return false;
        _db.Drivers.Remove(driver);
        await _db.SaveChangesAsync();
        return true;
    }

    public IReadOnlyList<Reservation> GetReservations() =>
        _db.Reservations.OrderBy(r => r.Start).ToList();

    public Reservation? GetReservationById(string id) =>
        _db.Reservations.FirstOrDefault(r => r.Id == id);

    public async Task<Reservation> CreateReservationAsync(Reservation reservation)
    {
        if (string.IsNullOrEmpty(reservation.Id))
            reservation.Id = $"res-{Guid.NewGuid():N}"[..12];
        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync();
        return reservation;
    }

    public async Task<Reservation?> UpdateReservationAsync(Reservation reservation)
    {
        var existing = await _db.Reservations.FindAsync(reservation.Id);
        if (existing is null) return null;
        _db.Entry(existing).CurrentValues.SetValues(reservation);
        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteReservationAsync(string id)
    {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation is null) return false;
        _db.Reservations.Remove(reservation);
        await _db.SaveChangesAsync();
        return true;
    }

    public IReadOnlyList<FleetNotification> GetNotifications() =>
        _db.Notifications.ToList();
}
