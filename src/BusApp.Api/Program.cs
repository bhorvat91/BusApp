using System.Text;
using BusApp.Application.DTOs;
using BusApp.Application.Interfaces;
using BusApp.Application.Services;
using BusApp.Domain.Entities;
using BusApp.Infrastructure.Data;
using BusApp.Infrastructure.Repositories;
using BusApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// EF Core + SQLite
builder.Services.AddDbContext<BusAppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=busapp.db"));

// Identity
builder.Services.AddIdentityCore<AppUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<BusAppDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "BusAppSuperSecretKeyForDev_ChangeMeInProduction!2026";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "BusApp",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "BusApp",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();

// Application services
builder.Services.AddScoped<IFleetRepository, EfFleetRepository>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<FleetService>();

var app = builder.Build();

// Seed database
await DbSeeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// ─── Public endpoints ───────────────────────────────────────────────────────

app.MapGet("/health", () => Results.Ok(new { ok = true, service = "busapp-api-dotnet" }));

// ─── Auth endpoints ─────────────────────────────────────────────────────────

app.MapPost("/api/auth/register", async (RegisterRequest request, UserManager<AppUser> userManager, ITokenService tokenService) =>
{
    var user = new AppUser
    {
        UserName = request.Email,
        Email = request.Email,
        FullName = request.FullName
    };

    var result = await userManager.CreateAsync(user, request.Password);
    if (!result.Succeeded)
        return Results.BadRequest(new { errors = result.Errors.Select(e => e.Description) });

    var token = tokenService.GenerateToken(user);
    var expiresMinutes = int.TryParse(app.Configuration["Jwt:ExpiresMinutes"], out var mins) ? mins : 480;
    return Results.Ok(new AuthResponse(token, user.Email!, user.FullName, DateTime.UtcNow.AddMinutes(expiresMinutes)));
});

app.MapPost("/api/auth/login", async (LoginRequest request, UserManager<AppUser> userManager, ITokenService tokenService) =>
{
    var user = await userManager.FindByEmailAsync(request.Email);
    if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        return Results.Unauthorized();

    var token = tokenService.GenerateToken(user);
    var expiresMinutes = int.TryParse(app.Configuration["Jwt:ExpiresMinutes"], out var mins) ? mins : 480;
    return Results.Ok(new AuthResponse(token, user.Email!, user.FullName, DateTime.UtcNow.AddMinutes(expiresMinutes)));
});

// ─── Protected fleet endpoints (read) ───────────────────────────────────────

var fleet = app.MapGroup("/api").RequireAuthorization();

fleet.MapGet("/overview", (FleetService service) => Results.Ok(service.GetOverview()));

fleet.MapGet("/calendar", (FleetService service) => Results.Ok(service.GetCalendar()));

fleet.MapGet("/reservations", (FleetService service) => Results.Ok(service.GetReservations()));

fleet.MapGet("/drivers/{driverId}", (string driverId, FleetService service) =>
{
    var detail = service.GetDriverDetail(driverId);
    return detail is not null ? Results.Ok(detail) : Results.NotFound(new { error = "Driver not found" });
});

// ─── CRUD: Buses ────────────────────────────────────────────────────────────

fleet.MapPost("/buses", async (CreateBusRequest request, IFleetRepository repo) =>
{
    var bus = new Bus
    {
        Code = request.Code,
        Plate = request.Plate,
        Capacity = request.Capacity,
        Status = request.Status,
        Location = new BusLocation { Lat = request.LocationLat, Lng = request.LocationLng, Label = request.LocationLabel },
        AssignedDriverId = request.AssignedDriverId,
        NextServiceKm = request.NextServiceKm
    };
    var created = await repo.CreateBusAsync(bus);
    return Results.Created($"/api/buses/{created.Id}", created);
});

fleet.MapPut("/buses/{id}", async (string id, UpdateBusRequest request, IFleetRepository repo) =>
{
    var bus = new Bus
    {
        Id = id,
        Code = request.Code,
        Plate = request.Plate,
        Capacity = request.Capacity,
        Status = request.Status,
        Location = new BusLocation { Lat = request.LocationLat, Lng = request.LocationLng, Label = request.LocationLabel },
        AssignedDriverId = request.AssignedDriverId,
        NextServiceKm = request.NextServiceKm
    };
    var updated = await repo.UpdateBusAsync(bus);
    return updated is not null ? Results.Ok(updated) : Results.NotFound(new { error = "Bus not found" });
});

fleet.MapDelete("/buses/{id}", async (string id, IFleetRepository repo) =>
{
    var deleted = await repo.DeleteBusAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound(new { error = "Bus not found" });
});

// ─── CRUD: Drivers ──────────────────────────────────────────────────────────

fleet.MapPost("/drivers", async (CreateDriverRequest request, IFleetRepository repo) =>
{
    var driver = new Driver
    {
        Name = request.Name,
        Phone = request.Phone,
        Status = request.Status,
        CurrentBusId = request.CurrentBusId,
        Shift = request.Shift,
        Licences = request.Licences
    };
    var created = await repo.CreateDriverAsync(driver);
    return Results.Created($"/api/drivers/{created.Id}", created);
});

fleet.MapPut("/drivers/{id}", async (string id, UpdateDriverRequest request, IFleetRepository repo) =>
{
    var driver = new Driver
    {
        Id = id,
        Name = request.Name,
        Phone = request.Phone,
        Status = request.Status,
        CurrentBusId = request.CurrentBusId,
        Shift = request.Shift,
        Licences = request.Licences
    };
    var updated = await repo.UpdateDriverAsync(driver);
    return updated is not null ? Results.Ok(updated) : Results.NotFound(new { error = "Driver not found" });
});

fleet.MapDelete("/drivers/{id}", async (string id, IFleetRepository repo) =>
{
    var deleted = await repo.DeleteDriverAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound(new { error = "Driver not found" });
});

// ─── CRUD: Reservations ─────────────────────────────────────────────────────

fleet.MapPost("/reservations", async (CreateReservationRequest request, IFleetRepository repo) =>
{
    var reservation = new Reservation
    {
        Title = request.Title,
        Customer = request.Customer,
        Route = request.Route,
        Start = request.Start,
        End = request.End,
        BusId = request.BusId,
        DriverId = request.DriverId,
        Status = request.Status,
        Pickup = request.Pickup,
        Dropoff = request.Dropoff
    };
    var created = await repo.CreateReservationAsync(reservation);
    return Results.Created($"/api/reservations/{created.Id}", created);
});

fleet.MapPut("/reservations/{id}", async (string id, UpdateReservationRequest request, IFleetRepository repo) =>
{
    var reservation = new Reservation
    {
        Id = id,
        Title = request.Title,
        Customer = request.Customer,
        Route = request.Route,
        Start = request.Start,
        End = request.End,
        BusId = request.BusId,
        DriverId = request.DriverId,
        Status = request.Status,
        Pickup = request.Pickup,
        Dropoff = request.Dropoff
    };
    var updated = await repo.UpdateReservationAsync(reservation);
    return updated is not null ? Results.Ok(updated) : Results.NotFound(new { error = "Reservation not found" });
});

fleet.MapDelete("/reservations/{id}", async (string id, IFleetRepository repo) =>
{
    var deleted = await repo.DeleteReservationAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound(new { error = "Reservation not found" });
});

app.Run();
