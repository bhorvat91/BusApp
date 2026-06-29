using Microsoft.AspNetCore.Identity;

namespace BusApp.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
}
