using System.Collections.Generic;
using System.Linq;
using AuraTween;
using Cysharp.Threading.Tasks;
using SoldByWizards.Input;
using SoldByWizards.Items;
using SoldByWizards.Maps;
using SoldByWizards.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoldByWizards.Computers
{
    // handles page navigation for profile and listings
    public class ComputerNavigation : MonoBehaviour
    {
        [SerializeField] private Computer _computer = null!;
        [SerializeField] private ComputerController _computerController = null!;
        [SerializeField] private PortalController _portalController = null!;
        [SerializeField] private InputController _inputController = null!;
        [SerializeField] private ListingPageSpamController _listingPageSpamController = null!;
        [SerializeField] private RectTransform _profilePanel = null!;
        [SerializeField] private RectTransform _listingsPanel = null!;
        [SerializeField] private Canvas _computerCanvas = null!;
        [SerializeField] private Canvas _batteryCanvas = null!;
        [SerializeField] private Image _listingNotificationImage = null!;
        [SerializeField] private Button _profileButton = null!;
        [SerializeField] private Button _listingsButton = null!;
        [SerializeField] private RectTransform _noItemsToListPanel = null!;
        [SerializeField] private RectTransform _itemsToListPanel = null!;
        [SerializeField] private RectTransform _currentlyListingPanel = null!;
        [SerializeField] private TextMeshProUGUI _itemsToSellTitleText = null!;
        [SerializeField] private TextMeshProUGUI _itemsToSellText = null!;
        [SerializeField] private CanvasGroup _uiCanvasGroup = null!;
        [SerializeField] private TweenManager _tweenManager = null!;
        [SerializeField] private RandomAudioPool? _popSoundAudioPool;
        [SerializeField] private List<TextMeshProUGUI> _typeText = new();
        [SerializeField] private float _typeTextStartPosition;
        [SerializeField] private float _typeTextEndPosition;
        [SerializeField] private float _typeAnimationLength;
        [SerializeField] private Ease _typeAnimationEase = Ease.Linear;

        private List<Item> _itemsWaitingForSale = new();
        private ComputerPage _activePage = ComputerPage.Profile;
        private ListingPageState _listingPageState = ListingPageState.NoItemsToList;
        private bool _selected = false;

        public void OnProfileButtonClicked()
        {
            Debug.Log("Switching to profile...");
            SwitchToPage(ComputerPage.Profile);
        }

        public void OnListingsButtonClicked()
        {
            Debug.Log("Switching to listings...");
            SwitchToPage(ComputerPage.Listings);
        }

        public void OnConfirmListingsButtonClicked()
        {
            Debug.Log("Confirming listings...");
            SetListingPageState(ListingPageState.CurrentlyListing);
            // ?
        }

        private void SwitchToPage(ComputerPage page, bool overrideCheck = false)
        {
            if (!overrideCheck && (!_selected || _activePage == page))
                return;

            _profilePanel.gameObject.SetActive(page == ComputerPage.Profile);
            _listingsPanel.gameObject.SetActive(page == ComputerPage.Listings);

            _activePage = page;
        }

        public void SetListingPageState(ListingPageState listingPageState)
        {
            _listingPageState = listingPageState;
            _listingNotificationImage.enabled = listingPageState == ListingPageState.ItemsToList || listingPageState == ListingPageState.CurrentlyListing;
            SetListingPanelObjects(listingPageState);

            if (listingPageState == ListingPageState.ItemsToList)
            {
                SetItemsToSellText(); // show items we have in the world
            }
            else if (listingPageState == ListingPageState.CurrentlyListing)
            {
                // disable buttons
                SetButtonInteractableState(false);

                // start listing items
                _listingPageSpamController.CreateListings(_itemsWaitingForSale);

                // re-enable import
                _inputController.SetInteractionInputState(false);
            }
            else if (listingPageState == ListingPageState.FinishedListingForDay)
            {
                // enable buttons
                SetButtonInteractableState(true);

                // re-enable export
                _inputController.SetInteractionInputState(true);
            }

            _computerCanvas.gameObject.SetActive(listingPageState != ListingPageState.InPortal);
            _batteryCanvas.gameObject.SetActive(listingPageState == ListingPageState.InPortal);

            if (listingPageState == ListingPageState.CurrentlyListing)
            {
                TypePromptAnimation().Forget();
            }

        }

        private void SetListingPanelObjects(ListingPageState listingPageState)
        {
            _currentlyListingPanel.gameObject.SetActive(listingPageState == ListingPageState.CurrentlyListing);
            _itemsToListPanel.gameObject.SetActive(listingPageState == ListingPageState.ItemsToList);
            _noItemsToListPanel.gameObject.SetActive(listingPageState == ListingPageState.NoItemsToList || listingPageState == ListingPageState.FinishedListingForDay);
        }
        public void SetButtonInteractableState(bool state)
        {
            _profileButton.interactable = state;
            _listingsButton.interactable = state;
        }

        private void SetItemsToSellText()
        {
            string descriptionText = "";
            foreach (var itemToSell in _itemsWaitingForSale)
            {
                descriptionText += $"{itemToSell.ItemSO.ItemName}\n";
            }

            _itemsToSellTitleText.text = $"Creating listing for {_itemsWaitingForSale.Count} items:";
            _itemsToSellText.text = descriptionText;
        }

        private void Start()
        {
            SwitchToPage(_activePage, true);
            SetListingPageState(ListingPageState.NoItemsToList);
        }

        private void OnEnable()
        {
            _computerController.OnComputerSelected += OnComputerSelected;
            _computerController.OnComputerDeselected += OnComputerDeselected;
            _computerController.OnItemsCollected += OnItemsCollected;
            _computerController.OnSpamTypingFinished += OnSpamTypingFinished;
            _portalController.OnPortalOpen += OnPortalOpen;
        }

        private void OnPortalOpen()
        {
            SetListingPageState(ListingPageState.InPortal);
        }

        private void OnSpamTypingFinished()
        {
            // finish "selling" items and send the event out
            SetListingPageState(ListingPageState.FinishedListingForDay);
            _computerController.ListItemsForSale(_itemsWaitingForSale.ToList());
            _itemsWaitingForSale.Clear();
        }

        private void OnDisable()
        {
            _computerController.OnComputerSelected -= OnComputerSelected;
            _computerController.OnComputerDeselected -= OnComputerDeselected;
            _computerController.OnItemsCollected -= OnItemsCollected;
            _computerController.OnSpamTypingFinished -= OnSpamTypingFinished;
            _portalController.OnPortalOpen -= OnPortalOpen;
        }

        private void OnItemsCollected(List<Item> items)
        {
            _itemsWaitingForSale = items;
            if (items.Count == 0)
            {
                SetListingPageState(ListingPageState.NoItemsToList);
            }
            else
            {
                SetListingPageState(ListingPageState.ItemsToList);
            }
        }

        private void OnComputerSelected(Computer computer)
        {
            _selected = true;
            if (_uiCanvasGroup != null)
            {
                _tweenManager.Run(1f, 0f, 0.5f, a => _uiCanvasGroup.alpha = a, Easer.InOutSine);
            }
        }

        private void OnComputerDeselected(Computer computer)
        {
            _selected = false;
            if (_uiCanvasGroup != null)
            {
                _tweenManager.Run(0f, 1f, 0.5f, a => _uiCanvasGroup.alpha = a, Easer.InOutSine);
            }
        }

        public async UniTask TypePromptAnimation()
        {
            for (int i = 0; i < _typeText.Count; i++)
            {
                _typeText[i].rectTransform.anchoredPosition = new Vector2(_typeTextStartPosition, _typeText[i].rectTransform.anchoredPosition.y);
                _typeText[i].gameObject.SetActive(true);
                await LerpTypeText(i);
            }

            await UniTask.WaitForSeconds(2f);

            for (int i = 0; i < _typeText.Count; i++)
            {
                LerpTypeTextBackwards(i).Forget();
            }

            // outro, and set inactive again
            await UniTask.WaitForSeconds(_typeAnimationLength);
            for (int i = 0; i < _typeText.Count; i++)
            {
                _typeText[i].gameObject.SetActive(false);
            }
        }

        public async UniTask LerpTypeText(int index)
        {
            // pop sound
            if (_popSoundAudioPool != null)
            {
                _popSoundAudioPool.PlayRandom();
            }

            var typeText = _typeText[index];

            var yPos = typeText.rectTransform.anchoredPosition.y;

            await _tweenManager.Run(_typeTextStartPosition, _typeTextEndPosition, _typeAnimationLength, (value) =>
            {
                typeText.rectTransform.anchoredPosition = new Vector2(value, yPos);
            }, _typeAnimationEase.ToProcedure());
        }

        public async UniTask LerpTypeTextBackwards(int index)
        {
            var typeText = _typeText[index];

            var yPos = typeText.rectTransform.anchoredPosition.y;

            await _tweenManager.Run(_typeTextEndPosition, _typeTextStartPosition, _typeAnimationLength, (value) =>
            {
                typeText.rectTransform.anchoredPosition = new Vector2(value, yPos);
            }, _typeAnimationEase.ToProcedure());
        }
    }
}
