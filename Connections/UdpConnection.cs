using DronePulse.Interfaces;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace DronePulse.Connections
{
    public class UdpConnection : IConnection
    {
        private readonly UdpClient _udpClient;
        private IPEndPoint? _remoteEndPoint;

        public UdpConnection(int localPort)
{
    try
    {
        Debug.WriteLine($"Creating UDP client on port {localPort}");
        _udpClient = new UdpClient(localPort);
        _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        Debug.WriteLine($"UDP client created successfully on port {localPort}");
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Error creating UDP client: {ex.Message}");
        throw;
    }
}

        public bool IsOpen => _udpClient.Client != null && _udpClient.Client.IsBound;

        public int BytesToRead => _udpClient.Available;

        public void Open()
        {
            // For UDP, the socket is "open" upon instantiation.
        }

        public Task ConnectAsync()
        {
            // UDP is connectionless, so just return a completed task
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            // No explicit disconnection needed for UDP
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _udpClient.Dispose();
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken token)
        {
            if (!IsOpen) return 0;

            var receiveTask = _udpClient.ReceiveAsync();
            var tcs = new TaskCompletionSource<UdpReceiveResult>();
            token.Register(() => tcs.TrySetCanceled());

            var completedTask = await Task.WhenAny(receiveTask, tcs.Task);

            if (completedTask == tcs.Task)
            {
                // Task was cancelled
                return 0;
            }

            var result = await receiveTask;
            _remoteEndPoint = result.RemoteEndPoint;
            Array.Copy(result.Buffer, 0, buffer, offset, result.Buffer.Length);
            return result.Buffer.Length;
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token = default)
        {
            if (!IsOpen || _remoteEndPoint == null)
                throw new InvalidOperationException("Connection is not open or remote endpoint is not set");

            try
            {
                await _udpClient.SendAsync(buffer, count, _remoteEndPoint);
            }
            catch (Exception ex)
            {
                throw new IOException("Error writing to UDP connection", ex);
            }
        }
    }
}
