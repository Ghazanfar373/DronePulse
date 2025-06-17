using DronePulse.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace DronePulse.Connections
{
    public class UdpConnection : IConnection
    {
        private readonly UdpClient _udpClient;
        private IPEndPoint? _remoteEndPoint;

        public UdpConnection(int localPort)
        {
            _udpClient = new UdpClient(localPort);
            _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public bool IsOpen => _udpClient.Client != null && _udpClient.Client.IsBound;

        public int BytesToRead => _udpClient.Available;

        public void Open()
        {
            // For UDP, the socket is "open" upon instantiation.
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
    }
}
