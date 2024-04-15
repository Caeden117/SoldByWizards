using SoldByWizards.Maps;
using SoldByWizards.Player;
using UnityEngine;
using UnityEngine.UI;

namespace SoldByWizards.UI
{
    public class PortalWarningUIController : MonoBehaviour
    {
        private static readonly Color _alphaColor = Color.red.WithA(0);

        [SerializeField] private TimedMapLoader _timedMapLoader = null!;
        [SerializeField] private PlayerController _playerController = null!;
        [SerializeField] private Graphic _targetGraphic = null!;

        private bool _isRunning;

        private void Start()
        {
            _timedMapLoader.OnTimerStarted += OnTimerStarted;
            _timedMapLoader.OnTimerEnded += OnTimerEnded;
        }

        private void OnTimerStarted() => _isRunning = true;

        private void OnTimerEnded()=> _isRunning = false;

        private void Update()
        {
            if (!(_isRunning && _playerController.transform.position.z > 0))
            {
                _targetGraphic.color = _alphaColor;
                return;
            }

            var color = Color.Lerp(_alphaColor, Color.red, Mathf.Sin(Time.time * 5));
            color.a *= Mathf.Clamp01(1f - _timedMapLoader.TimeRemaining / 10f);

            _targetGraphic.color = color;
        }

        private void OnDestroy()
        {
            _timedMapLoader.OnTimerStarted += OnTimerStarted;
            _timedMapLoader.OnTimerEnded += OnTimerEnded;
        }
    }
}
