# BusApp

BusApp je aplikacija za firme koje imaju autobuse — prati vozace, autobuse, rezervacije i raspored.

## Arhitektura

- **Backend**: ASP.NET Core 10 Web API (Clean Architecture)
- **Web frontend**: React + Vite (TypeScript)
- **Mobile**: React Native / Expo (TypeScript)

## Struktura

```
src/                          ← .NET solution
├── BusApp.Domain/            ← Entiteti i enumi
├── BusApp.Application/       ← Servisi, DTOs, interfejsi
├── BusApp.Infrastructure/    ← Implementacija repozitorija (InMemory / buduci EF Core)
└── BusApp.Api/               ← ASP.NET Core Minimal API

apps/
├── web/                      ← React dashboard (Vite)
└── mobile/                   ← Expo mobile app za vozace
```

## Pokretanje

### API (.NET)

```bash
cd src
dotnet run --project BusApp.Api
# API sluša na http://localhost:5110
```

### Web dashboard

```bash
npm install
npm run dev:web
# Otvori http://localhost:5173
```

### Mobile

```bash
npm run dev:mobile
```

## API Endpoints

| Method | URL                      | Opis                           |
|--------|--------------------------|--------------------------------|
| GET    | /health                  | Healthcheck                    |
| GET    | /api/overview            | Dashboard: firma, autobusi, vozaci, notifikacije |
| GET    | /api/calendar            | Kalendar rezervacija po danu   |
| GET    | /api/reservations        | Sve rezervacije sa detaljima   |
| GET    | /api/drivers/{driverId}  | Detalj vozaca sa rasporedom    |

## Verifikacija

```bash
# .NET build
cd src && dotnet build

# Web lint + build
npm run lint:web
npm run build:web

# Mobile typecheck
npm run typecheck:mobile
```

## Sljedeci koraci

- Entity Framework Core + SQL baza umjesto InMemory
- Autentikacija (JWT / Identity)
- Multi-tenant po firmi
- Live GPS stream (SignalR)
- Conflict detection za vozace i autobuse
- Servisni modul, dokumenti i izvjestaji
