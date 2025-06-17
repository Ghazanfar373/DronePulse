using System;
using System.Threading;
using System.Threading.Tasks;

namespace DronePulse.Interfaces
{
    public interface IConnection : IDisposable
    {
        bool IsOpen { get; }
        int BytesToRead { get; }

        void Open();
        Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken token);
    }
}
