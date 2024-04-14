using SoldByWizards.Items;
using UnityEngine;

namespace SoldByWizards.UI
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] private InventoryUISlot[] _inventoryUISlots;
        [SerializeField] private ItemsManager _itemsManager;

        private int _previouslySelectedSlot = -1;

        private void Start()
        {
            _itemsManager.OnItemSelected += OnItemSelected;
            _itemsManager.OnItemPickup += OnItemPickup;
            _itemsManager.OnItemDrop += OnItemDrop;

            OnItemSelected(0, null);
        }

        private void OnItemDrop(int itemIdx, Item item) => _inventoryUISlots[itemIdx].SetItem(null);

        private void OnItemPickup(int itemIdx, Item item) => _inventoryUISlots[itemIdx].SetItem(item);

        private void OnItemSelected(int itemIdx, ItemSO item)
        {
            if (_previouslySelectedSlot == itemIdx) return;

            if (_previouslySelectedSlot >= 0)
            {
                _inventoryUISlots[_previouslySelectedSlot].SetSelected(false);
            }

            _inventoryUISlots[itemIdx].SetSelected(true);

            _previouslySelectedSlot = itemIdx;
        }

        private void OnDestroy()
        {
            _itemsManager.OnItemSelected -= OnItemSelected;
            _itemsManager.OnItemPickup -= OnItemPickup;
            _itemsManager.OnItemDrop -= OnItemDrop;
        }
    }
}
