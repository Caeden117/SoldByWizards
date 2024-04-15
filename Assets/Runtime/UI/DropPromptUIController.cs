using AuraTween;
using SoldByWizards.Items;
using SoldByWizards.Player.Interactions;
using UnityEngine;

namespace SoldByWizards
{
    public class DropPromptUIController : MonoBehaviour
    {
        private const float _duration = 0.05f;

        [SerializeField] private ItemsManager _itemsManager;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _crosshairCanvasGroup;
        [SerializeField] private TweenManager _tweenManager;

        private bool _hasSeenPrompt = false;

        private void Start()
        {
            _itemsManager.OnItemPickup += OnItemPickup;
            _itemsManager.OnItemDrop += OnItemDrop;
        }

        private void OnItemPickup(int _, Item __)
        {
            if (_hasSeenPrompt) return;

            _tweenManager.Run(0f, 1f, _duration, a => _canvasGroup.alpha = a, Easer.InOutSine);
            _tweenManager.Run(1f, 0f, _duration, a => _crosshairCanvasGroup.alpha = a, Easer.InOutSine);
        }

        private void OnItemDrop(int _, Item __)
        {
            if (_hasSeenPrompt) return;

            _tweenManager.Run(1f, 0f, _duration, a => _canvasGroup.alpha = a, Easer.InOutSine);
            _tweenManager.Run(0f, 1f, _duration, a => _crosshairCanvasGroup.alpha = a, Easer.InOutSine);

            _hasSeenPrompt = true;
        }

        private void OnDestroy()
        {
            _itemsManager.OnItemPickup -= OnItemPickup;
            _itemsManager.OnItemDrop -= OnItemDrop;
        }
    }
}
