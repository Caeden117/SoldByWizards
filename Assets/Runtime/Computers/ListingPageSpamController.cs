using System.Collections.Generic;
using System.Linq;
using AuraTween;
using Cysharp.Threading.Tasks;
using SoldByWizards.Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace SoldByWizards.Computers
{
    public class ListingPageSpamController : MonoBehaviour
    {
        // TODO: Hook up dynamically
        [SerializeField] private Computer _computer;
        [SerializeField] private ComputerController _computerController = null!;
        [SerializeField] private TweenManager _tweenManager;
        [SerializeField] private ItemListing _itemListingTemplate;
        [SerializeField] private int _characterMultiplier = 1;
        [SerializeField] private float _scrollAmount = 700;
        [SerializeField] private float _itemListingContainerHeight = -5000f;
        [SerializeField] private float _pageScrollAnimationLength = 1f;
        [SerializeField] private Ease _pageScrollAnimationEase = Ease.Linear;

        private List<ItemListing> _spawnedItemListings = new();
        private int _totalCharactersTyped = 0;
        private int _currentItem = 0;
        private bool _receiveKeyboardInput = false;
        private bool _selected = false;
        private List<Item> _currentItems = new();

        // call when coming back from a teleport
        public void CreateListings(List<Item> items)
        {
            // TODO: Better state management
            if (_receiveKeyboardInput)
                return;

            _currentItems = items.ToList();

            foreach (var listing in _spawnedItemListings)
            {
                listing.gameObject.SetActive(false);
                Destroy(listing);
            }
            _spawnedItemListings.Clear();

            foreach (var item in _currentItems)
            {
                // create item
                var itemListing = Instantiate(_itemListingTemplate, _computer.ItemListingContainer);
                _spawnedItemListings.Add(itemListing);
                itemListing.Clear();
                itemListing.SetItem(item.ItemSO!);
            }

            // reset typing
            _totalCharactersTyped = 0;
            _receiveKeyboardInput = true;
            _currentItem = 0;

            // scroll to first object
            _computer.ItemListingContainer.anchoredPosition = new Vector2(_computer.ItemListingContainer.anchoredPosition.x, _itemListingContainerHeight);
        }

        private void OnEnable()
        {
            // i forgor how to unsubscribe from this
            InputSystem.onAnyButtonPress.Call(OnAnyButtonPress);

            _computerController.OnComputerSelected += OnComputerSelected;
            _computerController.OnComputerDeselected += OnComputerDeselected;
        }

        private void OnDisable()
        {
            _computerController.OnComputerSelected -= OnComputerSelected;
            _computerController.OnComputerDeselected -= OnComputerDeselected;
        }

        private void OnComputerSelected(Computer computer)
        {
            _selected = true;
        }

        private void OnComputerDeselected(Computer computer)
        {
            _selected = false;
        }

        private void ExitTypingMode()
        {
            // TODO: Animate out of this
            _totalCharactersTyped = 0;
            _receiveKeyboardInput = false;
            _currentItem = 0;
            _currentItems.Clear();

            Debug.Log("DONE TYPING MODE!");

            _computerController.FinishSpamTyping();
        }

        // This should have some way to cancel, i would like typing to get faster as you type more.
        // Right now it's hard to out-spam the scroll, but eventually it will be possible and multiple tweens may overlap
        private async UniTask ScrollToNextItem()
        {
            if (_computer == null)
                return;

            Debug.Log("Scrolling...");
            _currentItem++;

            await _tweenManager.Run(_computer.ItemListingContainer.anchoredPosition.y, _itemListingContainerHeight + (_currentItem * _scrollAmount), _pageScrollAnimationLength, (value) =>
            {
                _computer.ItemListingContainer.anchoredPosition = new Vector2(_computer.ItemListingContainer.anchoredPosition.x, value);
            }, _pageScrollAnimationEase.ToProcedure());
        }

        private void OnAnyButtonPress(InputControl ctrl)
        {
            if (!_receiveKeyboardInput || ctrl.device is not Keyboard || !_selected)
                return;

            _totalCharactersTyped += _characterMultiplier;
            SetTotalTextTyped(_totalCharactersTyped);
        }

        private void SetTotalTextTyped(int totalCharactersTyped)
        {
            int totalCharactersUsed = 0;
            for (int i = 0; i < _spawnedItemListings.Count; i++)
            {
                if (totalCharactersTyped - totalCharactersUsed <= 0)
                    break;

                var itemListing = _spawnedItemListings[i];

                if (itemListing.Saturated) // unnecessary to fill in text if it's already fully filled in.
                {
                    totalCharactersUsed += itemListing.GetTotalTextLength();
                    continue;
                }
                else
                {
                    itemListing.SetTextCount(totalCharactersTyped - totalCharactersUsed);
                    totalCharactersUsed += itemListing.GetTotalTextLength();
                }

                if (itemListing.Saturated)
                {
                    if (_currentItem + 1 == _spawnedItemListings.Count)
                    {
                        // final. break, we are DONE!
                        ExitTypingMode();
                        break;
                    }
                    // item listing is saturated, scroll to next
                    ScrollToNextItem().Forget();
                }
            }
        }
    }
}
