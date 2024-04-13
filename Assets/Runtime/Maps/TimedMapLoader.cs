using System;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using SoldByWizards.Glyphs;
using SoldByWizards.Player;
using UnityEngine;

namespace SoldByWizards.Maps
{
    public class TimedMapLoader : MonoBehaviour
    {
        [SerializeField] private float _mapLoadedDuration = 75;
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private GlyphAggregator _glyphAggregator;
        [SerializeField] private Transform _safetyTeleportPoint;

        [PublicAPI]
        public float TimeElapsed => (float)_timer.Elapsed.TotalSeconds;

        [PublicAPI]
        public float TimeRemaining => _mapLoadedDuration - TimeElapsed;

        private readonly Stopwatch _timer = new();

        [PublicAPI]
        public async UniTask LoadMapOnTimer()
        {
            if (_timer.IsRunning) return;

            _timer.Restart();

            await _mapLoader.LoadMapFromGlyphs();

            await UniTask.Delay(TimeSpan.FromSeconds(_mapLoadedDuration));

            _timer.Stop();

            await _mapLoader.UnloadMap();

            // dirtiest gamejam hack of my life
            if (!(_playerController.transform.position.z > 0)) return;

            // TODO: Drop all items in inventory after a safety teleport
            _playerController.transform.position = _safetyTeleportPoint.position;
            _playerCamera.transform.rotation = _safetyTeleportPoint.rotation;
        }
    }
}
