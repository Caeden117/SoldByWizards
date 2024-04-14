using System;
using System.Threading;
using JetBrains.Annotations;

namespace SoldByWizards.Util
{
    [PublicAPI]
    public class RecyclableCancellationTokenSource : IDisposable
    {
        public CancellationToken Token => _cancellationTokenSource?.Token ?? default;

        private CancellationTokenSource? _cancellationTokenSource = new();

        public void Cancel()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose() => _cancellationTokenSource?.Dispose();
    }
}
