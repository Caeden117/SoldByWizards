using System;
using System.Threading;
using JetBrains.Annotations;

namespace SoldByWizards.Util
{
    [PublicAPI]
    public class RecyclableCancellationTokenSource : IDisposable
    {
        public CancellationToken Token { get; private set; }

        private CancellationTokenSource? _cancellationTokenSource;

        public RecyclableCancellationTokenSource() => RecycleToken();

        public void Cancel()
        {
            _cancellationTokenSource?.Cancel();
            RecycleToken();
        }

        private void RecycleToken()
        {
            // :3
            Dispose();

            _cancellationTokenSource = new();
            Token = _cancellationTokenSource.Token;
        }

        public void Dispose() => _cancellationTokenSource?.Dispose();
    }
}
