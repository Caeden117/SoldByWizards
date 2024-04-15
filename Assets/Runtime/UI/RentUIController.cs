using System.Collections.Generic;
using AuraTween;
using Cysharp.Threading.Tasks;
using SoldByWizards.Game;
using SoldByWizards.Items;
using SoldByWizards.Maps;
using TMPro;
using UnityEngine;

namespace SoldByWizards.UI
{
    public class RentUIController : MonoBehaviour
    {
        [SerializeField] private TweenManager _tweenManager = null!;
        [SerializeField] private GameController _gameController = null!;
        [SerializeField] private TimedMapLoader _timedMapLoader = null!;
        [SerializeField] private RectTransform _barRectTransform = null!;
        [SerializeField] private RectTransform _barSpanContainer = null!;
        [SerializeField] private TimelineSpan _barSpanTemplate = null!;
        [SerializeField] private TextMeshProUGUI _moneyText = null!;
        [SerializeField] private TextMeshProUGUI _rentText = null!;
        [SerializeField] private float _barLength = 600f;

        private float _currentXPosition = 0f;
        private float? _currentSpanXPosition;
        private float _currentRent;
        private TimelineSpan? _currentSpan;
        private TimelineSpan? _previewSpan;
        private List<TimelineSpan> _spans = new();

        private void OnEnable()
        {
            _gameController.OnDayProgressUpdated += OnDayProgressUpdated;
            _gameController.OnDaySucceeded += OnDaySucceeded;
            _gameController.OnItemSold += OnItemSold;

            _currentRent = _gameController.CurrentRent;

            _timedMapLoader.OnTimerStarted += OnTimerStarted;
            _timedMapLoader.OnTimerEnded += OnTimerEnded;
        }


        private TimelineSpan CreateSpan(float xPosition, float initialWidth)
        {
            var span = Instantiate(_barSpanTemplate, _barSpanContainer);
            span.transform.SetAsFirstSibling();
            span.RectTransform.sizeDelta = new Vector2(initialWidth, span.RectTransform.sizeDelta.y);
            span.RectTransform.anchoredPosition = new Vector2(xPosition, span.RectTransform.anchoredPosition.y);
            _spans.Add(span);
            return span;
        }

        private void FinishSpan()
        {
            _currentSpan = null;
            _currentSpanXPosition = null;

            // Destroy preview span
            if (_previewSpan != null)
            {
                _previewSpan.gameObject.SetActive(false);
                Destroy(_previewSpan);
                _previewSpan = null;
            }
        }

        private void OnDisable()
        {
            _gameController.OnDayProgressUpdated -= OnDayProgressUpdated;
            _gameController.OnDaySucceeded -= OnDaySucceeded;
            _gameController.OnItemSold -= OnItemSold;

            _timedMapLoader.OnTimerStarted -= OnTimerStarted;
            _timedMapLoader.OnTimerEnded -= OnTimerEnded;
        }


        private void Clear()
        {
            foreach (var span in _spans)
            {
                if (span == null)
                    continue;

                span.gameObject.SetActive(false);
                Destroy(span);
            }

            _spans.Clear();
        }

        // start span
        private void OnTimerStarted()
        {
            _currentSpan = CreateSpan(_currentXPosition, 0f);
            _currentSpanXPosition = _currentXPosition;

            var expectedWidth = _barLength * (_timedMapLoader.MapLoadedDuration / _gameController.SecondsPerRentCycle);
            if (expectedWidth > _barLength - _currentXPosition)
                expectedWidth = _barLength - _currentXPosition;

            _previewSpan = CreateSpan(_currentXPosition, expectedWidth);
            _previewSpan.Image.color = Color.blue;
        }

        // end span
        private void OnTimerEnded()
        {
            FinishSpan();
        }


        private void OnItemSold(Item item, float salePrice)
        {
            _moneyText.text = $"${_gameController.CurrentMoney:N2}";

            FlyOutProfitText(salePrice)
                .AttachExternalCancellation(this.GetCancellationTokenOnDestroy())
                .Forget();
        }

        private void OnDaySucceeded()
        {
            _moneyText.text = $"${_gameController.CurrentMoney:N2}";
            _rentText.text = $"${_gameController.CurrentRent:N2}";
            Clear();
            FlyOutRentText(_currentRent)
                .AttachExternalCancellation(this.GetCancellationTokenOnDestroy())
                .Forget();
            _currentRent = _gameController.CurrentRent;
        }

        private void OnDayProgressUpdated(float progressAmount)
        {
            _currentXPosition = _barLength * progressAmount;
            _barRectTransform.anchoredPosition = new Vector2(_currentXPosition, _barRectTransform.anchoredPosition.y);

            if (_currentSpan != null && _currentSpanXPosition != null)
            {
                _currentSpan.RectTransform.sizeDelta = new Vector2(_currentXPosition - _currentSpanXPosition.Value, _currentSpan.RectTransform.sizeDelta.y);
            }
        }

        private async UniTask FlyOutProfitText(float saleAmount)
        {
            var duplicate = Instantiate(_moneyText, _moneyText.transform.parent);

            duplicate.text = $"+ ${saleAmount:N2}";

            var tween = _tweenManager.Run(Color.green, Color.green.WithA(0), 1f,
                c => duplicate.color = c, Easer.OutCubic);

            _tweenManager.Run(-25, -100f, 1f,
                y => duplicate.rectTransform.anchoredPosition = duplicate.rectTransform.anchoredPosition.WithY(y),
                Easer.OutExpo);

            await tween;

            DestroyImmediate(duplicate);
        }

        private async UniTask FlyOutRentText(float rent)
        {
            var duplicate = Instantiate(_moneyText, _moneyText.transform.parent);

            duplicate.text = $"- ${rent:N2}";

            var tween = _tweenManager.Run(Color.red, Color.red.WithA(0), 1f,
                c => duplicate.color = c, Easer.OutCubic);

            _tweenManager.Run(-25, -100f, 1f,
                y => duplicate.rectTransform.anchoredPosition = duplicate.rectTransform.anchoredPosition.WithY(y),
                Easer.OutExpo);

            await tween;

            DestroyImmediate(duplicate);
        }
    }
}
