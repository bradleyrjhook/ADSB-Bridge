using System;
using Newtonsoft.Json;

namespace ADSB_Server
{
    public class Aircraft
    {
        private DateTime lastSeen;
        private DateTime lastPositionUpdate;
        public bool HasNewData { get; internal set; } = false;

        public Aircraft(string hex)
        {
            Hex = hex;
            Type = "adsb_icao";
            Emergency = "none";
            LastSeen = DateTime.UtcNow;
            LastPositionUpdate = DateTime.UtcNow;
            MessageCount = 0;
        }

        // Basic identification
        public string Hex { get; private set; }
        public string Type { get; private set; }
        public string Flight { get; set; }

        // Altitude information
        public int? AltBaro { get; set; }
        public int? AltGeom { get; set; }

        // Speed and direction
        public double? Gs { get; set; }  // Ground speed
        public double? Ias { get; set; } // Indicated air speed
        public double? Mach { get; set; }
        public double? Track { get; set; }
        public double? MagHeading { get; set; }
        public double? TrueHeading { get; set; }
        public int? BaroRate { get; set; }

        // Identification codes
        public string Squawk { get; set; }
        public string Emergency { get; set; }
        public string Category { get; set; }

        // Position
        public double? Lat { get; set; }
        public double? Lon { get; set; }

        // Quality indicators
        public int? Nic { get; set; }
        public int? Rc { get; set; }
        public double? SeenPos { get; set; }
        public double? RDist { get; set; }
        public double? RDir { get; set; }
        public int? NicBaro { get; set; }
        public int? NacP { get; set; }
        public int? NacV { get; set; }
        public int? Sil { get; set; }
        public string SilType { get; set; }
        public int? Gva { get; set; }
        public int? Sda { get; set; }

        // Status indicators
        public int Alert { get; set; }
        public int Spi { get; set; }

        // Message statistics
        public int MessageCount { get; set; }

        // Timing
        public DateTime LastSeen
        {
            get => lastSeen;
            set
            {
                lastSeen = value;
                Seen = (DateTime.UtcNow - value).TotalSeconds;
            }
        }
        public DateTime LastPositionUpdate
        {
            get => lastPositionUpdate;
            set
            {
                lastPositionUpdate = value;
                SeenPos = (DateTime.UtcNow - value).TotalSeconds;
            }
        }
        public double Seen { get; private set; }
        public double Rssi { get; set; } = -30.0;

        public void ProcessMessage(string[] fields)
        {
            if (fields.Length < 10) return;

            MessageCount++;
            LastSeen = DateTime.UtcNow;
            HasNewData = true;  // Set flag when new data is received

            switch (fields[0])
            {
                case "MSG":
                    if (fields.Length >= 22)
                    {
                        int msgType = int.Parse(fields[1]);
                        ProcessMSGMessage(msgType, fields);
                    }
                    break;
                case "ID":
                case "AIR":
                case "STA":
                    if (fields.Length >= 11)
                    {
                        ProcessSTAMessage(fields[10]);
                    }
                    break;
            }
        }

        private void ProcessMSGMessage(int msgType, string[] fields)
        {
            switch (msgType)
            {
                case 1: // Identification
                    Flight = fields[10]?.Trim();
                    break;

                case 2: // Surface position
                case 3: // Airborne position
                    if (int.TryParse(fields[11], out int alt))
                    {
                        AltBaro = alt;
                        AltGeom = 0; // Approximate if not available
                    }

                    if (double.TryParse(fields[14], out double lat) &&
                        double.TryParse(fields[15], out double lon))
                    {
                        Lat = lat;
                        Lon = lon;
                        LastPositionUpdate = DateTime.UtcNow;
                    }

                    if (msgType == 3)
                    {
                        Emergency = fields[19] == "1" ? "emergency" : "none";
                        Alert = fields[18] == "1" ? 1 : 0;
                        Spi = fields[20] == "1" ? 1 : 0;
                    }
                    break;

                case 4: // Airborne velocity
                    if (double.TryParse(fields[12], out double gs))
                        Gs = gs;
                    if (double.TryParse(fields[13], out double trk))
                        Track = trk;
                    if (int.TryParse(fields[16], out int vr))
                        BaroRate = vr;
                    break;

                case 5: // Surveillance alt
                    if (int.TryParse(fields[11], out int alt5))
                        AltBaro = alt5;
                    Alert = fields[18] == "1" ? 1 : 0;
                    Spi = fields[20] == "1" ? 1 : 0;
                    break;

                case 6: // Surveillance ID
                    if (int.TryParse(fields[11], out int alt6))
                        AltBaro = alt6;
                    Squawk = fields[17];
                    Alert = fields[18] == "1" ? 1 : 0;
                    Emergency = fields[19] == "1" ? "emergency" : "none";
                    Spi = fields[20] == "1" ? 1 : 0;
                    break;

                case 7: // Air-to-air
                    if (int.TryParse(fields[11], out int alt7))
                        AltBaro = alt7;
                    break;
            }
        }

        private void ProcessSTAMessage(string status)
        {
            if (status == "RM")
            {
                LastSeen = DateTime.MinValue;  // This will cause it to be removed in next cleanup
            }
        }

        public object ToJson()
        {
            return new
            {
                now = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + DateTimeOffset.UtcNow.Millisecond / 1000.0,
                hex = Hex,
                type = Type,
                flight = Flight?.Trim() ?? "", 
                alt_baro = AltBaro ?? 0,
                alt_geom = AltGeom ?? 0,
                gs = Gs ?? 0.0,
                ias = Ias ?? 0.0,
                mach = Mach ?? 0.0,
                track = Track ?? 0.0,
                mag_heading = MagHeading ?? 0.0,
                true_heading = TrueHeading ?? 0.0,
                baro_rate = BaroRate ?? 0,
                squawk = Squawk ?? "0000",
                emergency = Emergency ?? "none",
                category = "",  // Changed from null to "A1" until I figure it out
                lat = Lat ?? 0.0,
                lon = Lon ?? 0.0,
                nic = Nic ?? 0,
                rc = Rc ?? 0,
                seen_pos = SeenPos ?? 0.0,
                r_dist = RDist ?? 0.0,
                r_dir = RDir ?? 0.0,
                nic_baro = NicBaro ?? 0,
                nac_p = NacP ?? 0,
                nac_v = NacV ?? 0,
                sil = Sil ?? 0,
                sil_type = "perhour",  // Changed from "unknown" to "perhour"
                gva = Gva ?? 0,
                sda = Sda ?? 0,
                alert = Alert,
                spi = Spi,
                messages = MessageCount,
                seen = Math.Round(Seen, 1),
                rssi = Rssi
            };
        }

        public DateTime LastUpdated => LastSeen;
    }
}