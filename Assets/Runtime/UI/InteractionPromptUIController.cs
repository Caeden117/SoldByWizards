using AuraTween;
using SoldByWizards.Player.Interactions;
using UnityEngine;

namespace SoldByWizards
{
    public class InteractionPromptUIController : MonoBehaviour
    {
        private const float _duration = 0.05f;

        [SerializeField] private InteractionsManager _interactionsManager;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _crosshairCanvasGroup;
        [SerializeField] private TweenManager _tweenManager;

        private void Start()
        {
            _interactionsManager.OnInteractionHover += OnInteractionHover;
            _interactionsManager.OnInteractionUnhover += OnInteractionUnhover;
        }

        private void OnInteractionHover()
        {
            _tweenManager.Run(0f, 1f, _duration, a => _canvasGroup.alpha = a, Easer.InOutSine);
            _tweenManager.Run(1f, 0f, _duration, a => _crosshairCanvasGroup.alpha = a, Easer.InOutSine);
        }

        private void OnInteractionUnhover()
        {
            _tweenManager.Run(1f, 0f, _duration, a => _canvasGroup.alpha = a, Easer.InOutSine);
            _tweenManager.Run(0f, 1f, _duration, a => _crosshairCanvasGroup.alpha = a, Easer.InOutSine);
        }

        private void OnDestroy()
        {
            _interactionsManager.OnInteractionHover -= OnInteractionHover;
            _interactionsManager.OnInteractionUnhover -= OnInteractionUnhover;
        }
    }
}
