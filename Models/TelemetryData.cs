using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DronePulse.Models
{
    public class TelemetryData : INotifyPropertyChanged
    {
        // Private fields
        private double _roll, _pitch, _yaw;
        private double _latitude, _longitude, _altitude;
        private string _fixType = "---";
        private int _satellitesVisible;
        private double _relativeAltitude, _heading;
        private string _cardinalDirection = "---";
        private string _status = "Disconnected";
        private double _groundSpeed;
        private double _airSpeed;
        private double _climbRate;
        private int _currentWaypoint;
        private int _totalWaypoints;
        private double _distanceToWaypoint;
        private double _batteryVoltage;
        private double _batteryRemaining;
        private string _flightMode = "---";
        private string _systemStatus = "---";
        private double _throttle;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Basic telemetry properties
        public double Roll { get => _roll; set { _roll = value; OnPropertyChanged(); OnPropertyChanged(nameof(RollText)); } }
        public double Pitch { get => _pitch; set { _pitch = value; OnPropertyChanged(); OnPropertyChanged(nameof(PitchText)); } }
        public double Yaw { get => _yaw; set { _yaw = value; OnPropertyChanged(); OnPropertyChanged(nameof(YawText)); } }
        public double Latitude { get => _latitude; set { _latitude = value; OnPropertyChanged(); OnPropertyChanged(nameof(LatitudeText)); } }
        public double Longitude { get => _longitude; set { _longitude = value; OnPropertyChanged(); OnPropertyChanged(nameof(LongitudeText)); } }
        public double Altitude { get => _altitude; set { _altitude = value; OnPropertyChanged(); OnPropertyChanged(nameof(AltitudeText)); } }
        public string FixType { get => _fixType; set { _fixType = value; OnPropertyChanged(); OnPropertyChanged(nameof(GpsStatusText)); } }
        public int SatellitesVisible { get => _satellitesVisible; set { _satellitesVisible = value; OnPropertyChanged(); OnPropertyChanged(nameof(GpsStatusText)); } }
        public double RelativeAltitude { get => _relativeAltitude; set { _relativeAltitude = value; OnPropertyChanged(); OnPropertyChanged(nameof(RelativeAltitudeText)); } }
        public double Heading { get => _heading; set { _heading = value; OnPropertyChanged(); OnPropertyChanged(nameof(HeadingText)); } }
        public string CardinalDirection { get => _cardinalDirection; set { _cardinalDirection = value; OnPropertyChanged(); OnPropertyChanged(nameof(HeadingText)); } }
        public string Status { get => _status; set { _status = value; OnPropertyChanged(); } }
        public double Throttle { get => _throttle; set { _throttle = value; OnPropertyChanged(); } }
        public string SystemStatus { get => _systemStatus; set { _systemStatus = value; OnPropertyChanged(); OnPropertyChanged(nameof(SystemStatusText)); } }

        // HUD properties
        public double GroundSpeed { get => _groundSpeed; set { _groundSpeed = value; OnPropertyChanged(); OnPropertyChanged(nameof(GroundSpeedText)); } }
        public double AirSpeed { get => _airSpeed; set { _airSpeed = value; OnPropertyChanged(); OnPropertyChanged(nameof(AirSpeedText)); } }
        public double ClimbRate { get => _climbRate; set { _climbRate = value; OnPropertyChanged(); OnPropertyChanged(nameof(ClimbRateText)); } }
        public int CurrentWaypoint { get => _currentWaypoint; set { _currentWaypoint = value; OnPropertyChanged(); OnPropertyChanged(nameof(WaypointText)); } }
        public int TotalWaypoints { get => _totalWaypoints; set { _totalWaypoints = value; OnPropertyChanged(); OnPropertyChanged(nameof(WaypointText)); } }
        public double DistanceToWaypoint { get => _distanceToWaypoint; set { _distanceToWaypoint = value; OnPropertyChanged(); OnPropertyChanged(nameof(DistanceToWaypointText)); } }
        public double BatteryVoltage { get => _batteryVoltage; set { _batteryVoltage = value; OnPropertyChanged(); OnPropertyChanged(nameof(BatteryText)); } }
        public double BatteryRemaining { get => _batteryRemaining; set { _batteryRemaining = value; OnPropertyChanged(); OnPropertyChanged(nameof(BatteryText)); } }
        public string FlightMode { get => _flightMode; set { _flightMode = value; OnPropertyChanged(); OnPropertyChanged(nameof(FlightModeText)); } }

        // Formatted text properties for display
        public string GpsStatusText => $"Fix: {FixType}, Sats: {SatellitesVisible}";
        public string HeadingText => $"Heading: {Heading:F1}° ({CardinalDirection})";
        public string RollText => $"Roll:  {Roll:F2}°";
        public string PitchText => $"Pitch: {Pitch:F2}°";
        public string YawText => $"Yaw:   {Yaw:F2}°";
        public string GroundSpeedText => $"Ground: {GroundSpeed:F1} m/s";
        public string AirSpeedText => $"Air: {AirSpeed:F1} m/s";
        public string ClimbRateText => $"Climb: {ClimbRate:F1} m/s";
        public string FlightModeText => $"Mode: {FlightMode}";
        public string BatteryText => $"Battery: {BatteryVoltage:F1}V ({(BatteryRemaining * 100):F0}%)";
        public string WaypointText => $"Waypoint: {CurrentWaypoint + 1}/{TotalWaypoints}";
        public string DistanceToWaypointText => $"Distance: {DistanceToWaypoint:F1} m";
        public string LatitudeText => $"Lat: {Latitude:F7}°";
        public string LongitudeText => $"Lon: {Longitude:F7}°";
        public string AltitudeText => $"Alt: {Altitude:F1} m";
        public string RelativeAltitudeText => $"Rel Alt: {RelativeAltitude:F1} m";
        public string SystemStatusText => $"Status: {SystemStatus}";
    }
}
