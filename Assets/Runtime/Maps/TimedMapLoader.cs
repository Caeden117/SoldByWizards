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
        [SerializeField] private PortalController _portalController;
        [SerializeField] private float _portalCooldown = 0.5f;

        public event Action OnTimerStarted;
        public event Action OnTimerEnded;

        [PublicAPI]
        public float TimeElapsed => _timer;

        [PublicAPI]
        public float TimeRemaining => _mapLoadedDuration - _timer;

        private float _timer = 0;
        private float _timeSinceLastPortalClose;

        [PublicAPI]
        public async UniTask LoadMapOnTimer(CancellationToken token = default)
        {
            if (Time.time - _timeSinceLastPortalClose < _portalCooldown) return;

            await _portalController.OpenAsync();

            _timer = 0;
            _timeSinceLastPortalClose = Time.time + _mapLoadedDuration + _portalCooldown;
            OnTimerStarted?.Invoke();

            await _mapLoader.LoadMapFromGlyphs();

            _glyphAggregator.ClearAllGlyphs();

            do
            {
                await UniTask.Yield();
                _timer += Time.deltaTime;
            }
            while (!token.IsCancellationRequested && _timer < _mapLoadedDuration);

            if (token.IsCancellationRequested)
            {
                _timeSinceLastPortalClose = Time.time + _portalCooldown;
            }

            OnTimerEnded?.Invoke();

            await _portalController.CloseAsync();

            // dirtiest gamejam hack of my life
            if (_playerController.transform.position.z > 0)
            {
                _playerController.Stop();
                _itemsManager.DeleteAllItems(true);

                _playerController.Rigidbody.position = _safetyTeleportPoint.position;
                _playerCamera.transform.rotation = _safetyTeleportPoint.rotation;
                await UniTask.Yield();
            }

            await _mapLoader.UnloadMap();

            _glyphAggregator.ResetGlyphDrawingStage();
        }
    }
}
