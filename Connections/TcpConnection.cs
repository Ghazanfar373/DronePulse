using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DronePulse.Interfaces;

namespace DronePulse.Connections
{
    public class TcpConnection : IConnection
    {
        private readonly string _host;
        private readonly int _port;
        private TcpClient? _client;
        private NetworkStream? _stream;

        public TcpConnection(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public bool IsOpen => _client?.Connected ?? false;

        public int BytesToRead => _client?.Available ?? 0;

        public void Open()
        {
            _client = new TcpClient();
            _client.Connect(_host, _port);
            _stream = _client.GetStream();
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken token)
        {
            if (_stream == null)
                return 0;

            return await _stream.ReadAsync(buffer, offset, count, token);
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _client?.Dispose();
        }
    }
}
