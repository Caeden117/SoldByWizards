using AuraTween;
using SoldByWizards.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoldByWizards.UI
{
    public class TimerUIController : MonoBehaviour
    {
        private const float _animTime = 1f;
        private const float _tickTime = 0.4f;

        [SerializeField] private TweenManager _tweenManager;
        [SerializeField] private TimedMapLoader _timedMapLoader;

        [Space, SerializeField] private Image _timerImage;
        [SerializeField] private TextMeshProUGUI[] _timerTexts;

        private Tween? activeTween;

        private bool _isRunning;
        private float _timeLength;
        private float _roundedTime;

        private void Start()
        {
            _timedMapLoader.OnTimerStarted += OnTimerStarted;
            _timedMapLoader.OnTimerEnded += OnTimerEnded;

            transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (!_isRunning) return;

            var rounded = Mathf.Ceil(_timedMapLoader.TimeRemaining);

            if (Mathf.Approximately(rounded, _roundedTime)) return;

            foreach (var timerText in _timerTexts)
            {
                timerText.text = Mathf.RoundToInt(rounded).ToString();
            }

            var progressA = _roundedTime / _timeLength;
            var progressB = rounded / _timeLength;

            _roundedTime = rounded;
            _tweenManager.Run(progressA, progressB, _tickTime, p => _timerImage.fillAmount = p,
                Easer.OutBack);
        }

        private void OnTimerStarted()
        {
            _isRunning = true;
            _roundedTime = Mathf.Ceil(_timedMapLoader.TimeRemaining);
            _timeLength = _roundedTime;
            foreach (var timerText in _timerTexts)
            {
                timerText.text = Mathf.RoundToInt(_roundedTime).ToString();
            }
            _timerImage.fillAmount = 1;

            activeTween?.Cancel();
            activeTween = _tweenManager.Run(0f, 1f, _animTime,
                s => transform.localScale = s * Vector3.one, Easer.OutElastic);
        }

        private void OnTimerEnded()
        {
            activeTween?.Cancel();
            activeTween = _tweenManager.Run(1f, 0f, _animTime,
                s => transform.localScale = s * Vector3.one, Easer.InElastic);
            activeTween.Value.SetOnComplete(() => _isRunning = false);
        }

        private void OnDestroy()
        {
            _timedMapLoader.OnTimerStarted -= OnTimerStarted;
            _timedMapLoader.OnTimerEnded -= OnTimerEnded;
        }
    }
}
