using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Timers;
using System.Drawing;
using System.Globalization;
using System.Threading;

namespace ADSB_Server
{
    public partial class MainForm : Form
    {
        private TcpListener tcpListener;
        private TcpClient basestationClient;
        private readonly Dictionary<string, Aircraft> aircraftList;
        private System.Timers.Timer cleanupTimer;
        private System.Timers.Timer jsonSendTimer;
        private static readonly object lockObject = new object();
        private const int staleTimeoutSeconds = 60; // Stale out time in seconds
        private bool isRunning = false;
        private List<TcpClient> connectedClients;
        private DateTime startTime;
        private string lastJsonData = string.Empty;
        private readonly string prototypeJson = @"{
  ""now"": 1740897867.249,
  ""hex"": ""ABCD12"",
  ""type"": ""adsb_icao"",
  ""flight"": ""UAL123"",
  ""alt_baro"": 20600,
  ""alt_geom"": 0,
  ""gs"": 281.0,
  ""ias"": 0,
  ""mach"": 0,
  ""track"": 70.0,
  ""mag_heading"": 0.0,
  ""true_heading"": 180.0,
  ""baro_rate"": 500,
  ""squawk"": ""7500"",
  ""emergency"": ""none"",
  ""category"": ""A1"",
  ""lat"": 37.7749,
  ""lon"": -122.4194,
  ""nic"": 8,
  ""rc"": 20,
  ""seen_pos"": 0.5,
  ""r_dist"": 10.0,
  ""r_dir"": 5.0,
  ""nic_baro"": 1,
  ""nac_p"": 10,
  ""nac_v"": 15,
  ""sil"": 2,
  ""sil_type"": ""perhour"",
  ""gva"": 2,
  ""sda"": 3,
  ""alert"": 1,
  ""spi"": 0,
  ""messages"": 100,
  ""seen"": 0.1,
  ""rssi"": -30.0
}";
        public MainForm()
        {
            InitializeComponent();
            lblCurrentTime.Text = $"UTC: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}";
            aircraftList = new Dictionary<string, Aircraft>();
            connectedClients = new List<TcpClient>();
            startTime = DateTime.UtcNow;
            this.FormClosing += MainForm_FormClosing;

            // Set window title without user info
            this.Text = "ADSB Bridge";

            // Initialize status
            UpdateConsole($"Application started...");
        }

        private string FormatUtcTime(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopActivities();
        }

        private void StartActivities()
        {
            if (!isRunning)
            {
                isRunning = true;
                startTime = DateTime.UtcNow;
                UpdateConsole($"Server started...");
                StartBasestationListener();
                StartTcpServer();
                StartCleanupTimer();
                StartJsonSendTimer();
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                txtBasestationIP.Enabled = false;
                txtBasestationPort.Enabled = false;
                txtServerIP.Enabled = false;
                txtServerPort.Enabled = false;
            }
        }

        private void StopActivities()
        {
            if (isRunning)
            {
                isRunning = false;
                UpdateConsole("Stopping server...");

                // Stop TCP listener
                if (tcpListener != null)
                {
                    tcpListener.Stop();
                    tcpListener = null;
                }

                // Close all client connections
                lock (connectedClients)
                {
                    foreach (var client in connectedClients.ToList())
                    {
                        try
                        {
                            client.Close();
                        }
                        catch { }
                    }
                    connectedClients.Clear();
                }

                // Close BaseStation connection
                if (basestationClient != null)
                {
                    basestationClient.Close();
                    basestationClient = null;
                }

                // Stop timers
                cleanupTimer?.Stop();
                jsonSendTimer?.Stop();

                UpdateBasestationStatus(false);
                UpdateServerStatus(false, false);
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                txtBasestationIP.Enabled = true;
                txtBasestationPort.Enabled = true;
                txtServerIP.Enabled = true;
                txtServerPort.Enabled = true;
                UpdateConsole("Server stopped");
            }
        }

        private void StartBasestationListener()
        {
            Task.Run(() =>
            {
                while (isRunning)
                {
                    try
                    {
                        string basestationIp = txtBasestationIP.Text;
                        int basestationPort = int.Parse(txtBasestationPort.Text);
                        basestationClient = new TcpClient(basestationIp, basestationPort);
                        NetworkStream ns = basestationClient.GetStream();
                        UpdateBasestationStatus(true);

                        byte[] myReadBuffer = new byte[1024];
                        StringBuilder messageBuilder = new StringBuilder();

                        while (isRunning)
                        {
                            int numberOfBytesRead = ns.Read(myReadBuffer, 0, myReadBuffer.Length);
                            string data = Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead);

                            messageBuilder.Append(data);
                            string messages = messageBuilder.ToString();
                            string[] lines = messages.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                            messageBuilder.Clear();
                            if (!messages.EndsWith("\n"))
                            {
                                messageBuilder.Append(lines[lines.Length - 1]);
                                lines = lines.Take(lines.Length - 1).ToArray();
                            }

                            foreach (string message in lines)
                            {
                                if (!string.IsNullOrEmpty(message))
                                {
                                    UpdateConsole(message);
                                    ProcessMessage(message);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (isRunning)
                        {
                            UpdateConsole($"Error: {ex.Message}");
                            Task.Delay(5000).Wait(); // Wait before retry
                        }
                        UpdateBasestationStatus(false);
                    }
                }
            });
        }

        private void ProcessMessage(string message)
        {
            string[] fields = message.Split(',');
            if (fields.Length < 5) return;

            string icao24 = fields[4];
            if (!aircraftList.ContainsKey(icao24))
            {
                aircraftList[icao24] = new Aircraft(icao24);
            }

            aircraftList[icao24].ProcessMessage(fields);
        }

        private void StartTcpServer()
        {
            try
            {
                string serverIp = txtServerIP.Text;
                int serverPort = int.Parse(txtServerPort.Text);
                tcpListener = new TcpListener(IPAddress.Parse(serverIp), serverPort);
                tcpListener.Start();
                UpdateConsole($"TCP Server started on {serverIp}:{serverPort}");
                lblServerStatus.BackColor = Color.Orange;

                Task.Run(() =>
                {
                    while (isRunning)
                    {
                        try
                        {
                            TcpClient client = tcpListener.AcceptTcpClient();
                            lock (connectedClients)
                            {
                                connectedClients.Add(client);
                                UpdateServerStatus(true, true);
                            }
                            Task.Run(() => HandleClient(client));
                        }
                        catch (SocketException ex)
                        {
                            if (isRunning)
                            {
                                UpdateConsole($"TCP Server error: {ex.Message}");
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                UpdateConsole($"Failed to start TCP Server: {ex.Message}");
            }
        }

        private void HandleClient(TcpClient client)
        {
            string clientEndPoint = "";
            try
            {
                clientEndPoint = ((IPEndPoint)client.Client?.RemoteEndPoint)?.ToString() ?? "unknown";
                UpdateConsole($"Client connected from {clientEndPoint}");

                NetworkStream stream = client.GetStream();

                // Send initial data
                string json = GetJsonData();
                if (!string.IsNullOrEmpty(json))
                {
                    byte[] data = Encoding.UTF8.GetBytes(json + "\n");
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }

                // Keep connection alive until disconnected
                while (client.Connected && isRunning)
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                UpdateConsole($"Client error: {ex.Message}");
            }
            finally
            {
                UpdateConsole($"Client disconnected from {clientEndPoint}");
                lock (connectedClients)
                {
                    connectedClients.Remove(client);
                }
                try
                {
                    client.Close();
                }
                catch { }
                UpdateServerStatus(connectedClients.Count > 0, false);
            }
        }
        private string GetJsonData()
        {
            var jsonList = new StringBuilder();
            foreach (var aircraft in aircraftList.Values)
            {
                jsonList.AppendLine(JsonConvert.SerializeObject(aircraft.ToJson(), Formatting.Indented));
            }
            return jsonList.ToString();
        }

        private void StartJsonSendTimer()
        {
            jsonSendTimer = new System.Timers.Timer(1000); // Check every second instead of 100ms
            jsonSendTimer.Elapsed += (sender, e) => SendJsonDataToClients();
            jsonSendTimer.Start();
        }

        private Dictionary<string, string> lastAircraftJson = new Dictionary<string, string>();

        private void SendJsonDataToClients()
        {
            lock (lockObject)
            {
                foreach (var aircraft in aircraftList.Values)
                {
                    // Only send if aircraft has new data
                    if (aircraft.HasNewData)
                    {
                        string currentJson = JsonConvert.SerializeObject(aircraft.ToJson(), Formatting.Indented);

                        lock (connectedClients)
                        {
                            foreach (var client in connectedClients.ToList())
                            {
                                try
                                {
                                    if (!client.Connected)
                                    {
                                        connectedClients.Remove(client);
                                        continue;
                                    }

                                    NetworkStream stream = client.GetStream();
                                    byte[] data = Encoding.UTF8.GetBytes(currentJson + "\n");
                                    stream.Write(data, 0, data.Length);
                                    stream.Flush();
                                }
                                catch (Exception)
                                {
                                    client.Close();
                                    connectedClients.Remove(client);
                                }
                            }
                        }

                        // Reset the flag after sending
                        aircraft.HasNewData = false;
                    }
                }
            }
        }



        private void StartCleanupTimer()
        {
            cleanupTimer = new System.Timers.Timer(10000);
            cleanupTimer.Elapsed += (sender, e) => CleanupStaleAircraft();
            cleanupTimer.Start();
        }

        private void CleanupStaleAircraft()
        {
            lock (lockObject)
            {
                var staleAircraft = aircraftList.Values
                    .Where(a => (DateTime.UtcNow - a.LastUpdated).TotalSeconds > staleTimeoutSeconds)
                    .Select(a => a.Hex)  // Changed from Icao24 to Hex
                    .ToList();

                foreach (var hex in staleAircraft)  // Changed variable name to match
                {
                    aircraftList.Remove(hex);
                    UpdateConsole($"Removed stale aircraft {hex}");
                }
            }
        }

        private void UpdateConsole(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateConsole), message);
                return;
            }

            txtConsole.AppendText($"{FormatUtcTime(DateTime.UtcNow)}: {message}{Environment.NewLine}");

            // Keep only last 1000 lines
            var lines = txtConsole.Lines;
            if (lines.Length > 1000)
            {
                txtConsole.Lines = lines.Skip(lines.Length - 1000).ToArray();
            }

            txtConsole.SelectionStart = txtConsole.TextLength;
            txtConsole.ScrollToCaret();
        }

        private void ClearConsole()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ClearConsole));
                return;
            }
            txtConsole.Clear();
        }

        private void UpdateBasestationStatus(bool connected)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(UpdateBasestationStatus), connected);
                return;
            }
            lblBasestationStatus.BackColor = connected ? Color.Green : Color.Red;
        }

        private void UpdateServerStatus(bool connected, bool sending)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool, bool>(UpdateServerStatus), connected, sending);
                return;
            }
            lblServerStatus.BackColor = sending ? Color.Green : connected ? Color.Orange : Color.Red;
            lblServerStatus.Text = $"Server Status";
        }

        private void btnExampleJson_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                UpdateConsole("Server is not running. Cannot send test JSON.");
                return;
            }

            SendExampleJson();
            UpdateConsole("Test JSON sent to connected client");
        }

        private void SendExampleJson()
        {
            lock (connectedClients)
            {
                foreach (var client in connectedClients.ToList())
                {
                    try
                    {
                        if (!client.Connected)
                        {
                            connectedClients.Remove(client);
                            continue;
                        }

                        NetworkStream stream = client.GetStream();
                        byte[] data = Encoding.UTF8.GetBytes(prototypeJson + "\n");
                        stream.Write(data, 0, data.Length);
                        stream.Flush();
                    }
                    catch (Exception)
                    {
                        client.Close();
                        connectedClients.Remove(client);
                    }
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartActivities();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopActivities();
        }

        private void btnViewJson_Click(object sender, EventArgs e)
        {
            string json = GetJsonData();
            UpdateConsole(json);
        }

        private void btnClearConsole_Click(object sender, EventArgs e)
        {
            ClearConsole();
            UpdateConsole($"Console cleared at {FormatUtcTime(DateTime.UtcNow)}");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = $"UTC: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}";
        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            var stats = new StringBuilder();
            stats.AppendLine("Current Statistics:");
            stats.AppendLine($"Server running since: {FormatUtcTime(startTime)}");
            stats.AppendLine($"Total aircraft tracked: {aircraftList.Count}");
            stats.AppendLine($"Connected clients: {connectedClients.Count}");
            UpdateConsole(stats.ToString());
        }
    }
}