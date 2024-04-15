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
        [SerializeField] private bool _scaleParent = false;
        [SerializeField] private float _scaleAmount = 0.5f;

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
                if (_scaleParent)
                    transform.parent.localScale = Vector3.one;
                else
                    transform.localScale = Vector3.one;
                return;
            }

            float intensity = Mathf.Sin(Time.time * 5) * 0.5f + 0.5f;
            intensity *= Mathf.Clamp01(1f - _timedMapLoader.TimeRemaining / 10f);

            var color = Color.Lerp(_alphaColor, Color.red, intensity);

            _targetGraphic.color = color;

            float scale = 1.0f + _scaleAmount * intensity;
            if (_scaleParent)
                transform.parent.localScale = new Vector3(scale, scale, scale);
            else
                transform.localScale = new Vector3(scale, scale, scale);
        }

        private void OnDestroy()
        {
            _timedMapLoader.OnTimerStarted += OnTimerStarted;
            _timedMapLoader.OnTimerEnded += OnTimerEnded;
        }
    }
}
