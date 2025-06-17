using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using DronePulse.Interfaces;

namespace DronePulse.Connections
{
    public class SerialConnection : IConnection
    {
        private readonly SerialPort _serialPort;

        public SerialConnection(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
        }

        public bool IsOpen => _serialPort.IsOpen;

        public int BytesToRead => _serialPort.BytesToRead;

        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }


        public Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken token)
        {
            return _serialPort.BaseStream.ReadAsync(buffer, offset, count, token);
        }

        public void Dispose()
        {
                //_serialPort.Close();
            _serialPort.Dispose();
        }
    }
}
