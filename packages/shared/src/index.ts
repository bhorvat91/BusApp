export type FleetStatus =
  | 'on-route'
  | 'available'
  | 'break'
  | 'service'
  | 'offline'

export type ReservationStatus = 'planned' | 'active' | 'completed'

export interface CompanyProfile {
  id: string
  name: string
  city: string
  supportPhone: string
}

export interface BusLocation {
  lat: number
  lng: number
  label: string
}

export interface Bus {
  id: string
  code: string
  plate: string
  capacity: number
  status: FleetStatus
  location: BusLocation
  assignedDriverId: string
  nextServiceKm: number
}

export interface Driver {
  id: string
  name: string
  phone: string
  status: FleetStatus
  currentBusId: string
  shift: string
  licences: string[]
}

export interface Reservation {
  id: string
  title: string
  customer: string
  route: string
  start: string
  end: string
  busId: string
  driverId: string
  status: ReservationStatus
  pickup: string
  dropoff: string
}

export interface FleetNotification {
  id: string
  level: 'info' | 'warning' | 'critical'
  title: string
  description: string
}

export const company: CompanyProfile = {
  id: 'firm-1',
  name: 'BusApp Fleet',
  city: 'Sarajevo',
  supportPhone: '+387 61 222 333',
}

export const buses: Bus[] = [
  {
    id: 'bus-1',
    code: 'A-12',
    plate: 'K88-J-245',
    capacity: 52,
    status: 'on-route',
    location: { lat: 43.8563, lng: 18.4131, label: 'Stup terminal' },
    assignedDriverId: 'drv-1',
    nextServiceKm: 1400,
  },
  {
    id: 'bus-2',
    code: 'B-07',
    plate: 'M34-T-188',
    capacity: 48,
    status: 'available',
    location: { lat: 43.8606, lng: 18.4214, label: 'Depot Ilidza' },
    assignedDriverId: 'drv-2',
    nextServiceKm: 4200,
  },
  {
    id: 'bus-3',
    code: 'C-03',
    plate: 'A11-E-099',
    capacity: 58,
    status: 'service',
    location: { lat: 43.8478, lng: 18.3564, label: 'Service centar' },
    assignedDriverId: 'drv-3',
    nextServiceKm: 120,
  },
]

export const drivers: Driver[] = [
  {
    id: 'drv-1',
    name: 'Emir Hadzic',
    phone: '+387 62 100 201',
    status: 'on-route',
    currentBusId: 'bus-1',
    shift: '06:00 - 14:00',
    licences: ['D', 'CPC'],
  },
  {
    id: 'drv-2',
    name: 'Lejla Kovac',
    phone: '+387 62 100 202',
    status: 'available',
    currentBusId: 'bus-2',
    shift: '08:00 - 16:00',
    licences: ['D', 'CPC', 'First aid'],
  },
  {
    id: 'drv-3',
    name: 'Tarik Music',
    phone: '+387 62 100 203',
    status: 'service',
    currentBusId: 'bus-3',
    shift: '07:00 - 15:00',
    licences: ['D'],
  },
]

export const reservations: Reservation[] = [
  {
    id: 'res-1',
    title: 'Aerodrom transfer',
    customer: 'Hotel Hills',
    route: 'Sarajevo Airport → Old Town',
    start: '2026-06-26T08:00:00+02:00',
    end: '2026-06-26T09:15:00+02:00',
    busId: 'bus-1',
    driverId: 'drv-1',
    status: 'active',
    pickup: 'Sarajevo Airport',
    dropoff: 'Bascarsija',
  },
  {
    id: 'res-2',
    title: 'Corporate shuttle',
    customer: 'Tech Park',
    route: 'Ilidza → Marijin Dvor',
    start: '2026-06-26T10:30:00+02:00',
    end: '2026-06-26T11:20:00+02:00',
    busId: 'bus-2',
    driverId: 'drv-2',
    status: 'planned',
    pickup: 'Ilidza',
    dropoff: 'Marijin Dvor',
  },
  {
    id: 'res-3',
    title: 'Tourist day trip',
    customer: 'Visit Bosnia',
    route: 'Sarajevo → Mostar',
    start: '2026-06-27T07:00:00+02:00',
    end: '2026-06-27T20:00:00+02:00',
    busId: 'bus-1',
    driverId: 'drv-1',
    status: 'planned',
    pickup: 'Hotel Europe',
    dropoff: 'Mostar bus station',
  },
  {
    id: 'res-4',
    title: 'School transfer',
    customer: 'Osnovna skola Centar',
    route: 'Centar → Vogosca',
    start: '2026-06-28T07:30:00+02:00',
    end: '2026-06-28T08:20:00+02:00',
    busId: 'bus-2',
    driverId: 'drv-2',
    status: 'planned',
    pickup: 'Centar',
    dropoff: 'Vogosca',
  },
]

export const notifications: FleetNotification[] = [
  {
    id: 'notif-1',
    level: 'warning',
    title: 'Bus C-03 ulazi u servis',
    description: 'Potrebno potvrditi zamjensko vozilo za popodnevnu smjenu.',
  },
  {
    id: 'notif-2',
    level: 'info',
    title: 'Nova rezervacija potvrdena',
    description: 'Corporate shuttle za Tech Park dodijeljen vozacu Lejla Kovac.',
  },
  {
    id: 'notif-3',
    level: 'critical',
    title: 'Detektovan konflikt rasporeda',
    description: 'Provjeriti preklapanje za Emir Hadzic na 27.06.',
  },
]

export const statusLabels: Record<FleetStatus, string> = {
  'on-route': 'Na ruti',
  available: 'Slobodan',
  break: 'Pauza',
  service: 'Servis',
  offline: 'Van mreze',
}

export const statusColors: Record<FleetStatus, string> = {
  'on-route': '#14b8a6',
  available: '#22c55e',
  break: '#f59e0b',
  service: '#ef4444',
  offline: '#64748b',
}

export function getBusById(busId: string) {
  return buses.find((bus) => bus.id === busId)
}

export function getDriverById(driverId: string) {
  return drivers.find((driver) => driver.id === driverId)
}

export function reservationsByDay() {
  return reservations.reduce<Record<string, Reservation[]>>((groups, reservation) => {
    const key = reservation.start.slice(0, 10)
    groups[key] ??= []
    groups[key].push(reservation)
    return groups
  }, {})
}

export function dashboardSummary() {
  return {
    activeReservations: reservations.filter((item) => item.status === 'active').length,
    plannedReservations: reservations.filter((item) => item.status === 'planned').length,
    activeBuses: buses.filter((item) => item.status === 'on-route').length,
    availableDrivers: drivers.filter((item) => item.status === 'available').length,
  }
}
