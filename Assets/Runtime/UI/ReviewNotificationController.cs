using System.Collections.Generic;
using AuraTween;
using Cysharp.Threading.Tasks;
using SoldByWizards.Reviews;
using SoldByWizards.Util;
using UnityEngine;

namespace SoldByWizards.UI
{
    public class ReviewNotificationController : MonoBehaviour
    {
        [SerializeField] private ReviewController _reviewController = null!;
        [SerializeField] private TweenManager _tweenManager = null!;
        [SerializeField] private ReviewSlot _reviewSlot = null!;
        [SerializeField] private float _notificationDisplayTime;
        [SerializeField] private float _hiddenPosition;
        [SerializeField] private float _visiblePosition;
        [SerializeField] private float _animationDuration = 1f;
        [SerializeField] private Ease _inEase = Ease.Linear;
        [SerializeField] private Ease _outEase = Ease.Linear;
        [SerializeField] private RandomAudioPool? _popAudioPool;

        // steam reference
        private Queue<GeneratedReview> _reviewQueue = new();
        private bool _goingThroughQueue = false;

        private void OnReviewGenerated(GeneratedReview review)
        {
            _reviewQueue.Enqueue(review);

            if (!_goingThroughQueue)
            {
                DequeueLoop().Forget();
            }
        }

        private async UniTask DequeueLoop()
        {
            _goingThroughQueue = true;

            // wait a little before starting
            await UniTask.Delay(2000);

            while (_reviewQueue.TryDequeue(out var generatedReview))
            {
                _reviewSlot.SetReview(generatedReview);
                if (_popAudioPool != null)
                {
                    _popAudioPool.PlayRandom();
                }

                await MakeNotificationVisible();

                await UniTask.WaitForSeconds(_notificationDisplayTime);

                await MakeNotificationHidden();
            }
            _goingThroughQueue = false;
        }

        // hide notification on start
        private void Start()
        {
            _reviewSlot.RectTransform.anchoredPosition = new Vector2(_reviewSlot.RectTransform.anchoredPosition.x, _hiddenPosition);
        }

        private void OnEnable()
        {
            _reviewController.OnReviewGenerated += OnReviewGenerated;
        }

        private void OnDisable()
        {
            _reviewController.OnReviewGenerated -= OnReviewGenerated;
        }

        public async UniTask MakeNotificationVisible()
        {
            await _tweenManager.Run(0f, 1f, _animationDuration, (value) =>
            {
                var yPosition = Mathf.Lerp(_hiddenPosition, _visiblePosition, value);
                _reviewSlot.RectTransform.anchoredPosition = new Vector2(_reviewSlot.RectTransform.anchoredPosition.x, yPosition);
                _reviewSlot.RectTransform.localScale = new Vector3(1, value, 1);
            }, _inEase.ToProcedure());
        }

        public async UniTask MakeNotificationHidden()
        {
            await _tweenManager.Run(0f, 1f, _animationDuration, (value) =>
            {
                var yPosition = Mathf.Lerp(_visiblePosition, _hiddenPosition, value);
                _reviewSlot.RectTransform.anchoredPosition = new Vector2(_reviewSlot.RectTransform.anchoredPosition.x, yPosition);
                _reviewSlot.RectTransform.localScale = new Vector3(1, 1 - value, 1);
            }, _outEase.ToProcedure());
        }
    }
}
