using BusApp.Application.Interfaces;
using BusApp.Application.Services;
using BusApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddSingleton<IFleetRepository, InMemoryFleetRepository>();
builder.Services.AddScoped<FleetService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.MapGet("/health", () => Results.Ok(new { ok = true, service = "busapp-api-dotnet" }));

app.MapGet("/api/overview", (FleetService service) => Results.Ok(service.GetOverview()));

app.MapGet("/api/calendar", (FleetService service) => Results.Ok(service.GetCalendar()));

app.MapGet("/api/reservations", (FleetService service) => Results.Ok(service.GetReservations()));

app.MapGet("/api/drivers/{driverId}", (string driverId, FleetService service) =>
{
    var detail = service.GetDriverDetail(driverId);
    return detail is not null ? Results.Ok(detail) : Results.NotFound(new { error = "Driver not found" });
});

app.Run();
