# DronePulse

A C# application for interfacing with drones using the MAVLink protocol. This tool connects to a flight controller via serial communication to display and monitor real-time telemetry data including GPS coordinates, altitude, and attitude information.

## Features

- Connects to flight controllers using MAVLink protocol
- Displays real-time telemetry data:
  - GPS position and fix status
  - Altitude and relative altitude
  - Attitude (roll, pitch, yaw)
  - Ground speed and heading
- Interactive Terminal User Interface (TUI) dashboard
- Real-time display of key telemetry:
  - Attitude (Roll, Pitch, Yaw)
  - GPS (Latitude, Longitude, Altitude)
  - Global Position (Relative Altitude, Heading)
- Configuration via an external `appsettings.json` file

## Prerequisites

- .NET 6.0 or later
- A compatible flight controller with MAVLink support
- USB connection to the flight controller

## Installation

1. Clone this repository:
   ```
   git clone https://github.com/Ghazanfar373/DronePulse.git
   ```
2. Navigate to the project directory:
   ```
   cd DronePulse
   ```
3. Build the solution:
   ```
   dotnet build
   ```

## Usage

1. Connect your flight controller to your computer via USB
2. Run the application:
   ```
   dotnet run
   ```
3. The application will attempt to connect to the default serial port (COM8) at 115200 baud
4. To specify a different port or baud rate, modify the `portName` and `baudRate` variables in `Program.cs`

## Configuration

Configuration is handled via the `appsettings.json` file. Edit this file to match your serial port setup:

```json
{
  "SerialPortSettings": {
    "PortName": "COM8",
    "BaudRate": 115200
  }
}
```

- `PortName`: The COM port your flight controller is connected to (e.g., `COM3` on Windows or `/dev/ttyUSB0` on Linux).
- `BaudRate`: The serial communication speed. 115200 is common, but adjust if your device uses a different rate.

## MAVLink Messages Supported

- `MSG_ID_ATTITUDE` (ID: 30): Attitude and orientation data
- `MSG_ID_GPS_RAW_INT` (ID: 24): Raw GPS data
- `MSG_ID_GLOBAL_POSITION_INT` (ID: 33): Global position data
- `MSG_ID_GPS_STATUS` (ID: 25): GPS status information
- `MSG_ID_HEARTBEAT` (ID: 0): Heartbeat message (commented out by default)

## License

This project is open source and available under the [MIT License](LICENSE).

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Author

[Your Name] - [Your GitHub Profile]

## Acknowledgments

- MAVLink development team for the MAVLink protocol
- .NET team for the excellent framework
