# BusApp

BusApp je pocetna MVP osnova za:
- web aplikaciju za administraciju i dispecere
- mobile aplikaciju za vozace
- API servis za buduci backend i integraciju GPS/rezervacija

## Struktura

- `/home/runner/work/BusApp/BusApp/apps/web` – web dashboard za pracenje flote i kalendar planiranja
- `/home/runner/work/BusApp/BusApp/apps/mobile` – mobile ekran vozaca za dnevni raspored i status
- `/home/runner/work/BusApp/BusApp/apps/api` – Express API sa mock podacima i pregledima
- `/home/runner/work/BusApp/BusApp/packages/shared` – zajednicki tipovi i seed podaci

## MVP sta je implementirano

- pregled autobusa i vozaca sa statusima
- lokacije autobusa i vozaca kroz mock GPS podatke
- kalendarski pregled rezervacija i planiranih voznji
- dnevni mobilni prikaz za vozaca
- osnovni alerti i operativne notifikacije
- API endpointi za overview, kalendar, rezervacije i detalj vozaca

## Pokretanje

U root direktoriju:

```bash
npm install
```

Web:

```bash
npm run dev:web
```

Mobile:

```bash
npm run dev:mobile
```

API:

```bash
npm run dev:api
```

## Verifikacija

```bash
npm run lint
npm run build
npm run typecheck:mobile
```

## Sljedeci koraci

- autentikacija i multi-tenant podrka po firmi
- live GPS stream umjesto mock podataka
- conflict detection za vozace i autobuse u realnom vremenu
- servisni modul, dokumenti i izvjestaji
- integracija notifikacija i eventualno billing
