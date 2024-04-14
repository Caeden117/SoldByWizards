using System;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using SoldByWizards.Glyphs;
using SoldByWizards.Items;
using SoldByWizards.Player;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SoldByWizards.Maps
{
    public class TimedMapLoader : MonoBehaviour
    {
        public float MapLoadedDuration => _mapLoadedDuration;

        [SerializeField] private float _mapLoadedDuration = 75;
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private GlyphAggregator _glyphAggregator;
        [SerializeField] private Transform _safetyTeleportPoint;
        [SerializeField] private ItemsManager _itemsManager;

        public event Action OnTimerStarted;
        public event Action OnTimerEnded;

        [PublicAPI]
        public float TimeElapsed => (float)_timer.Elapsed.TotalSeconds;

        [PublicAPI]
        public float TimeRemaining => _mapLoadedDuration - TimeElapsed;

        private readonly Stopwatch _timer = new();

        [PublicAPI]
        public async UniTask LoadMapOnTimer(CancellationToken token = default)
        {
            if (_timer.IsRunning) return;

            _timer.Restart();
            OnTimerStarted?.Invoke();

            await _mapLoader.LoadMapFromGlyphs();

            var target = TimeSpan.FromSeconds(_mapLoadedDuration);

            do
            {
                await UniTask.Yield();
            }
            while (!token.IsCancellationRequested && _timer.Elapsed < target);

            _timer.Stop();
            OnTimerEnded?.Invoke();

            await _mapLoader.UnloadMap();

            // dirtiest gamejam hack of my life
            if (!(_playerController.transform.position.z > 0)) return;

            _playerController.Stop();
            _itemsManager.DropAllItems();

            await UniTask.Yield();

            _playerController.transform.position = _safetyTeleportPoint.position;
            _playerCamera.transform.rotation = _safetyTeleportPoint.rotation;
        }
    }
}
