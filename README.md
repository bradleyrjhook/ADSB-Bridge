# ADSB Bridge

ADSB Bridge is a Windows application that bridges BaseStation's SBS-1 protocol (port 30003) to a JSON format for use with RadarSites and other compatible applications. It acts as a middleman, converting real-time aircraft data from BaseStation format into standardized JSON messages.

<img src="https://iili.io/33Exs8G.png" width="800" height="auto">

## Features

- Converts BaseStation format (SBS-1) to JSON in real-time
- Handles multiple TCP client connections
- Supports MSG, ID, AIR, and STA message types
- Provides real-time aircraft tracking information
- Includes aircraft cleanup for stale data (60-second timeout)
- Shows server and connection status
- Includes statistics and monitoring capabilities

## Configuration

### BaseStation Connection
- Default IP: 127.0.0.1
- Default Port: 30003

### JSON Server
- Default IP: 127.0.0.1
- Default Port: 30154

## JSON Format
```json
{
  "now": 1740897867.249,
  "hex": "ABCD12",
  "type": "adsb_icao",
  "flight": "",
  "alt_baro": 0,
  "alt_geom": 0,
  "gs": 281.0,
  "ias": 0.0,
  "mach": 0.0,
  "track": 70.0,
  "mag_heading": 0.0,
  "true_heading": 0.0,
  "baro_rate": -1280,
  "squawk": "0000",
  "emergency": "none",
  "category": "A1",
  "lat": 0.0,
  "lon": 0.0,
  "nic": 0,
  "rc": 0,
  "seen_pos": 0.0,
  "r_dist": 0.0,
  "r_dir": 0.0,
  "nic_baro": 0,
  "nac_p": 0,
  "nac_v": 0,
  "sil": 0,
  "sil_type": "perhour",
  "gva": 0,
  "sda": 0,
  "alert": 0,
  "spi": 0,
  "messages": 2,
  "seen": 0.0,
  "rssi": -30.0
}
```

## Usage

1. Start RTL1090 software and ensure it's outputting data on port 30003
2. Launch ADSB Bridge
3. Configure the BaseStation IP and port if different from defaults
4. Configure the Server IP and port for JSON output
5. Click "Start" to begin bridging data
6. Connect your client application (like RadarSites) to the JSON server port

## Features
- Real-time data conversion
- Connection status indicators
- Console logging
- JSON message viewing
- Statistics monitoring
- Sample JSON testing capability
- Automatic stale aircraft cleanup

## Status Indicators
- 🔴 Red: Not connected
- 🟡 Orange: Server ready, no client
- 🟢 Green: Connected and transmitting

## System Requirements
- Windows OS
- .NET Framework 4.7.2 or higher
- RTL1090, Dump1090 or compatible ADS-B decoder software

## Developer Notes
- Aircraft data is only transmitted when new information is received
- Stale aircraft are removed after 60 seconds of inactivity
- JSON messages include Unix timestamp with millisecond precision
