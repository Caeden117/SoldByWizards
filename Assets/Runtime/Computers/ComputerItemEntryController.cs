using System.Collections.Generic;
using SoldByWizards.Items;
using UnityEngine;
using UnityEngine.UI;

namespace SoldByWizards.Computers
{
    public class ComputerItemEntryController : MonoBehaviour
    {
        // TODO: Hook up dynamically
        [SerializeField] public List<ItemSO> CurrentItems = new();
        [SerializeField] private ItemListing _itemListingTemplate;
        [SerializeField] private RectTransform _itemListingContainer;

        private List<ItemListing> _spawnedItemListings = new();

        // call when coming back from a teleport
        public void CreateListings()
        {
            foreach (var listing in _spawnedItemListings)
            {
                listing.gameObject.SetActive(false);
                Destroy(listing);
            }
            _spawnedItemListings.Clear();

            foreach (var item in CurrentItems)
            {
                // create item
                var itemListing = Instantiate(_itemListingTemplate, _itemListingContainer);
                _spawnedItemListings.Add(itemListing);
                itemListing.Clear();
                itemListing.SetItem(item);
            }
        }

        private void Start()
        {
            CreateListings();
        }
    }
}
