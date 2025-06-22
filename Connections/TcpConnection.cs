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

        public async Task ConnectAsync()
        {
            if (_client == null || !_client.Connected)
            {
                _client = new TcpClient();
                await _client.ConnectAsync(_host, _port);
                _stream = _client.GetStream();
            }
        }

        public async Task DisconnectAsync()
        {
            if (_client != null)
            {
                _stream?.Dispose();
                _client.Dispose();
                _client = null;
                _stream = null;
            }
            await Task.CompletedTask;
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken token)
        {
            if (_stream == null)
                return 0;

            return await _stream.ReadAsync(buffer, offset, count, token);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token = default)
        {
            if (_client == null || !_client.Connected)
                throw new InvalidOperationException("TCP client is not connected");

            try
            {
                var stream = _client.GetStream();
                await stream.WriteAsync(buffer, offset, count, token);
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is IOException)
            {
                await DisconnectAsync();
                throw new IOException("Error writing to TCP connection", ex);
            }
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _client?.Dispose();
        }
    }
}
