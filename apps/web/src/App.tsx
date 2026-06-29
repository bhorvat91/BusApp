import { useEffect, useState } from 'react'
import './App.css'

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5110'

interface BusLocation {
  lat: number
  lng: number
  label: string
}

interface BusDto {
  id: string
  code: string
  plate: string
  capacity: number
  status: string
  location: BusLocation
  assignedDriverId: string
  nextServiceKm: number
}

interface DriverDto {
  id: string
  name: string
  phone: string
  status: string
  currentBusId: string
  shift: string
  licences: string[]
}

interface NotificationDto {
  id: string
  level: string
  title: string
  description: string
}

interface ReservationDto {
  id: string
  title: string
  route: string
  start: string
  end: string
  busId: string
  driverId: string
  status: string
}

interface OverviewData {
  company: { id: string; name: string; city: string; supportPhone: string }
  summary: {
    activeReservations: number
    plannedReservations: number
    activeBuses: number
    availableDrivers: number
  }
  buses: BusDto[]
  drivers: DriverDto[]
  notifications: NotificationDto[]
}

interface CalendarData {
  days: Record<string, ReservationDto[]>
}

const statusLabels: Record<string, string> = {
  'on-route': 'Na ruti',
  available: 'Slobodan',
  break: 'Pauza',
  service: 'Servis',
  offline: 'Van mreze',
}

function App() {
  const [overview, setOverview] = useState<OverviewData | null>(null)
  const [calendar, setCalendar] = useState<CalendarData | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    Promise.all([
      fetch(`${API_URL}/api/overview`).then((r) => r.json()),
      fetch(`${API_URL}/api/calendar`).then((r) => r.json()),
    ])
      .then(([overviewData, calendarData]) => {
        setOverview(overviewData)
        setCalendar(calendarData)
      })
      .catch((err) => setError(err.message))
  }, [])

  if (error) {
    return (
      <main className="app-shell">
        <section className="hero">
          <div>
            <h1>BusApp</h1>
            <p className="hero-copy">
              Greška pri učitavanju podataka: {error}
            </p>
            <p className="meta">Pokreni .NET API: cd src &amp;&amp; dotnet run --project BusApp.Api</p>
          </div>
        </section>
      </main>
    )
  }

  if (!overview || !calendar) {
    return (
      <main className="app-shell">
        <section className="hero">
          <div>
            <h1>BusApp</h1>
            <p className="hero-copy">Učitavanje...</p>
          </div>
        </section>
      </main>
    )
  }

  const { company, summary, buses, drivers, notifications } = overview
  const findDriver = (id: string) => drivers.find((d) => d.id === id)
  const findBus = (id: string) => buses.find((b) => b.id === id)

  return (
    <main className="app-shell">
      <section className="hero">
        <div>
          <p className="eyebrow">Fleet operations cockpit</p>
          <h1>{company.name}</h1>
          <p className="hero-copy">
            Web dashboard za firme koje prate autobuse, vozace, rezervacije i
            dnevni raspored iz jednog mjesta.
          </p>
        </div>
        <div className="hero-card">
          <span className="hero-label">Operativni centar</span>
          <strong>{company.city}</strong>
          <p>Podrska: {company.supportPhone}</p>
        </div>
      </section>

      <section className="stats-grid">
        <article className="stat-card">
          <span>Aktivne voznje</span>
          <strong>{summary.activeReservations}</strong>
        </article>
        <article className="stat-card">
          <span>Planirane rezervacije</span>
          <strong>{summary.plannedReservations}</strong>
        </article>
        <article className="stat-card">
          <span>Autobusi na ruti</span>
          <strong>{summary.activeBuses}</strong>
        </article>
        <article className="stat-card">
          <span>Slobodni vozaci</span>
          <strong>{summary.availableDrivers}</strong>
        </article>
      </section>

      <section className="content-grid">
        <article className="panel panel-wide">
          <div className="panel-header">
            <div>
              <p className="eyebrow">Live pregled</p>
              <h2>Autobusi i lokacije</h2>
            </div>
            <span className="pill">.NET API</span>
          </div>
          <div className="fleet-grid">
            {buses.map((bus) => {
              const driver = findDriver(bus.assignedDriverId)

              return (
                <div className="fleet-card" key={bus.id}>
                  <div className="fleet-head">
                    <div>
                      <strong>{bus.code}</strong>
                      <p>{bus.plate}</p>
                    </div>
                    <span className="status-badge" data-status={bus.status}>
                      {statusLabels[bus.status] || bus.status}
                    </span>
                  </div>
                  <p className="location">{bus.location.label}</p>
                  <p className="meta">
                    {bus.location.lat.toFixed(4)}, {bus.location.lng.toFixed(4)}
                  </p>
                  <p className="meta">Vozac: {driver?.name}</p>
                  <p className="meta">Servis za {bus.nextServiceKm} km</p>
                </div>
              )
            })}
          </div>
        </article>

        <article className="panel">
          <div className="panel-header">
            <div>
              <p className="eyebrow">Alarmi</p>
              <h2>Operativne notifikacije</h2>
            </div>
          </div>
          <div className="alert-list">
            {notifications.map((item) => (
              <div className="alert-card" key={item.id} data-level={item.level}>
                <strong>{item.title}</strong>
                <p>{item.description}</p>
              </div>
            ))}
          </div>
        </article>

        <article className="panel panel-wide">
          <div className="panel-header">
            <div>
              <p className="eyebrow">Kalendar</p>
              <h2>Rezervacije i planovi</h2>
            </div>
          </div>
          <div className="calendar-grid">
            {Object.entries(calendar.days).map(([day, items]) => (
              <div className="calendar-day" key={day}>
                <strong>{day}</strong>
                {items.map((item) => (
                  <div className="calendar-event" key={item.id}>
                    <span>
                      {item.start.slice(11, 16)} - {item.end.slice(11, 16)}
                    </span>
                    <strong>{item.title}</strong>
                    <p>{item.route}</p>
                    <p>
                      {findBus(item.busId)?.code} · {findDriver(item.driverId)?.name}
                    </p>
                  </div>
                ))}
              </div>
            ))}
          </div>
        </article>

        <article className="panel">
          <div className="panel-header">
            <div>
              <p className="eyebrow">Vozaci</p>
              <h2>Aktivne smjene</h2>
            </div>
          </div>
          <div className="driver-list">
            {drivers.map((driver) => (
              <div className="driver-card" key={driver.id}>
                <div>
                  <strong>{driver.name}</strong>
                  <p>{driver.shift}</p>
                </div>
                <span className="status-badge" data-status={driver.status}>
                  {statusLabels[driver.status] || driver.status}
                </span>
                <p className="meta">
                  {findBus(driver.currentBusId)?.code} · {driver.licences.join(', ')}
                </p>
              </div>
            ))}
          </div>
        </article>
      </section>
    </main>
  )
}

export default App
