using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DronePulse.Models
{
    public class TelemetryData : INotifyPropertyChanged
    {
        private double _roll, _pitch, _yaw;
        private double _latitude, _longitude, _altitude;
        private string _fixType = "---";
        private int _satellitesVisible;
        private double _relativeAltitude, _heading;
        private string _cardinalDirection = "---";
        private string _status = "Disconnected";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        // Composite properties for data binding
        public string GpsStatusText => $"Fix: {FixType}, Sats: {SatellitesVisible}";
        public string HeadingText => $"Heading: {Heading:F1}° ({CardinalDirection})";

        public string RollText => $"Roll:  {Roll:F2}°";
        public string PitchText => $"Pitch: {Pitch:F2}°";
        public string YawText => $"Yaw:   {Yaw:F2}°";
        public string LatitudeText => $"Lat: {Latitude:F7}°";
        public string LongitudeText => $"Lon: {Longitude:F7}°";
        public string AltitudeText => $"Alt: {Altitude:F1} m";
        public string RelativeAltitudeText => $"Rel Alt: {RelativeAltitude:F1} m";
    }
}
