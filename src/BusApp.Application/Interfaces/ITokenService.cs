using BusApp.Domain.Entities;

namespace BusApp.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(AppUser user);
}
