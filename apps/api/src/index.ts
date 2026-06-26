import cors from 'cors'
import express from 'express'
import { z } from 'zod'

import {
  buses,
  company,
  dashboardSummary,
  drivers,
  getBusById,
  getDriverById,
  notifications,
  reservations,
  reservationsByDay,
  statusLabels,
} from '../../../packages/shared/src/index.js'

const app = express()
const port = process.env.PORT || 4000

app.use(cors())
app.use(express.json())

app.get('/health', (_req, res) => {
  res.json({ ok: true, service: 'busapp-api' })
})

app.get('/api/overview', (_req, res) => {
  res.json({
    company,
    summary: dashboardSummary(),
    buses,
    drivers,
    notifications,
  })
})

app.get('/api/calendar', (_req, res) => {
  res.json({
    days: reservationsByDay(),
    reservations,
  })
})

app.get('/api/reservations', (_req, res) => {
  const items = reservations.map((reservation) => ({
    ...reservation,
    bus: getBusById(reservation.busId),
    driver: getDriverById(reservation.driverId),
  }))

  res.json(items)
})

app.get('/api/drivers/:driverId', (req, res) => {
  const params = z.object({ driverId: z.string().min(1) }).safeParse(req.params)

  if (!params.success) {
    res.status(400).json({ error: 'Invalid driver id' })
    return
  }

  const driver = getDriverById(params.data.driverId)

  if (!driver) {
    res.status(404).json({ error: 'Driver not found' })
    return
  }

  const assignedBus = getBusById(driver.currentBusId)
  const schedule = reservations.filter((reservation) => reservation.driverId === driver.id)

  res.json({
    ...driver,
    bus: assignedBus,
    schedule,
    statusLabel: statusLabels[driver.status],
  })
})

app.listen(port, () => {
  console.log(`BusApp API listening on port ${port}`)
})
