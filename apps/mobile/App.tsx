import { StatusBar } from 'expo-status-bar';
import { useEffect, useState } from 'react';
import {
  ActivityIndicator,
  Pressable,
  SafeAreaView,
  ScrollView,
  StyleSheet,
  Text,
  View,
} from 'react-native';

const API_URL = process.env.EXPO_PUBLIC_API_URL || 'http://localhost:5110';

interface BusDto {
  id: string;
  code: string;
  plate: string;
  capacity: number;
  location: { label: string };
}

interface ReservationDto {
  id: string;
  title: string;
  route: string;
  start: string;
  pickup: string;
  dropoff: string;
}

interface DriverDetailDto {
  id: string;
  name: string;
  phone: string;
  status: string;
  statusLabel: string;
  shift: string;
  licences: string[];
  bus: BusDto | null;
  schedule: ReservationDto[];
}

type FleetStatus = 'on-route' | 'available' | 'break';

const statusLabels: Record<string, string> = {
  'on-route': 'Na ruti',
  available: 'Slobodan',
  break: 'Pauza',
  service: 'Servis',
  offline: 'Van mreze',
};

export default function App() {
  const [driverData, setDriverData] = useState<DriverDetailDto | null>(null);
  const [status, setStatus] = useState<FleetStatus>('on-route');
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch(`${API_URL}/api/drivers/drv-1`)
      .then((r) => r.json())
      .then((data: DriverDetailDto) => {
        setDriverData(data);
        setStatus(data.status as FleetStatus);
      })
      .catch((err) => setError(err.message));
  }, []);

  if (error) {
    return (
      <SafeAreaView style={styles.safeArea}>
        <View style={styles.centered}>
          <Text style={styles.sectionTitle}>Greška</Text>
          <Text style={styles.infoText}>{error}</Text>
          <Text style={styles.infoText}>Pokreni API: cd src &amp;&amp; dotnet run --project BusApp.Api</Text>
        </View>
      </SafeAreaView>
    );
  }

  if (!driverData) {
    return (
      <SafeAreaView style={styles.safeArea}>
        <View style={styles.centered}>
          <ActivityIndicator size="large" color="#0f766e" />
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={styles.safeArea}>
      <StatusBar style="dark" />
      <ScrollView contentContainerStyle={styles.container}>
        <View style={styles.heroCard}>
          <Text style={styles.eyebrow}>Driver mobile</Text>
          <Text style={styles.title}>{driverData.name}</Text>
          <Text style={styles.subtitle}>
            Današnji raspored, status vožnje i dijeljenje lokacije za dispečerski tim.
          </Text>
          <View style={styles.statusRow}>
            <View style={styles.statusPill}>
              <Text style={styles.statusText}>{statusLabels[status] || status}</Text>
            </View>
            <Text style={styles.shiftText}>Smjena: {driverData.shift}</Text>
          </View>
        </View>

        {driverData.bus && (
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>Dodijeljeni autobus</Text>
            <View style={styles.infoCard}>
              <Text style={styles.infoTitle}>{driverData.bus.code}</Text>
              <Text style={styles.infoText}>{driverData.bus.plate}</Text>
              <Text style={styles.infoText}>Kapacitet: {driverData.bus.capacity} mjesta</Text>
              <Text style={styles.infoText}>Lokacija: {driverData.bus.location.label}</Text>
            </View>
          </View>
        )}

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Brza promjena statusa</Text>
          <View style={styles.actionRow}>
            {(['on-route', 'available', 'break'] as FleetStatus[]).map((nextStatus) => (
              <Pressable
                key={nextStatus}
                onPress={() => setStatus(nextStatus)}
                style={[
                  styles.actionButton,
                  status === nextStatus && styles.actionButtonActive,
                ]}
              >
                <Text
                  style={[
                    styles.actionButtonText,
                    status === nextStatus && styles.actionButtonTextActive,
                  ]}
                >
                  {statusLabels[nextStatus]}
                </Text>
              </Pressable>
            ))}
          </View>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Današnje i naredne vožnje</Text>
          {driverData.schedule.map((reservation) => (
            <View key={reservation.id} style={styles.tripCard}>
              <Text style={styles.tripTime}>
                {reservation.start.slice(0, 16).replace('T', ' ')}
              </Text>
              <Text style={styles.infoTitle}>{reservation.title}</Text>
              <Text style={styles.infoText}>{reservation.route}</Text>
              <Text style={styles.infoText}>
                {reservation.pickup} → {reservation.dropoff}
              </Text>
            </View>
          ))}
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Checklist prije polaska</Text>
          <View style={styles.checklistCard}>
            <Text style={styles.infoText}>• potvrdi pregled vozila</Text>
            <Text style={styles.infoText}>• uključi dijeljenje lokacije</Text>
            <Text style={styles.infoText}>• provjeri broj putnika i rezervaciju</Text>
            <Text style={styles.infoText}>• javi dispečeru eventualno kašnjenje</Text>
          </View>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  safeArea: {
    flex: 1,
    backgroundColor: '#eef4fb',
  },
  centered: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  container: {
    padding: 20,
    gap: 18,
  },
  heroCard: {
    backgroundColor: '#0f172a',
    padding: 20,
    borderRadius: 24,
    gap: 10,
  },
  eyebrow: {
    color: '#99f6e4',
    fontSize: 12,
    fontWeight: '700',
    textTransform: 'uppercase',
    letterSpacing: 1.2,
  },
  title: {
    color: '#f8fafc',
    fontSize: 30,
    fontWeight: '700',
  },
  subtitle: {
    color: '#cbd5e1',
    fontSize: 15,
    lineHeight: 22,
  },
  statusRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    gap: 12,
    flexWrap: 'wrap',
  },
  statusPill: {
    backgroundColor: '#ccfbf1',
    paddingHorizontal: 12,
    paddingVertical: 8,
    borderRadius: 999,
  },
  statusText: {
    color: '#115e59',
    fontWeight: '700',
  },
  shiftText: {
    color: '#f8fafc',
    fontWeight: '600',
  },
  section: {
    gap: 12,
  },
  sectionTitle: {
    color: '#0f172a',
    fontSize: 20,
    fontWeight: '700',
  },
  infoCard: {
    backgroundColor: '#ffffff',
    borderRadius: 20,
    padding: 18,
    gap: 6,
  },
  infoTitle: {
    color: '#0f172a',
    fontSize: 18,
    fontWeight: '700',
  },
  infoText: {
    color: '#475569',
    fontSize: 15,
    lineHeight: 22,
  },
  actionRow: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 10,
  },
  actionButton: {
    paddingHorizontal: 14,
    paddingVertical: 10,
    borderRadius: 999,
    backgroundColor: '#ffffff',
  },
  actionButtonActive: {
    backgroundColor: '#0f766e',
  },
  actionButtonText: {
    color: '#0f172a',
    fontWeight: '700',
  },
  actionButtonTextActive: {
    color: '#f8fafc',
  },
  tripCard: {
    backgroundColor: '#ffffff',
    borderRadius: 20,
    padding: 18,
    gap: 6,
  },
  tripTime: {
    color: '#0f766e',
    fontWeight: '700',
  },
  checklistCard: {
    backgroundColor: '#ffffff',
    borderRadius: 20,
    padding: 18,
    gap: 8,
  },
});
