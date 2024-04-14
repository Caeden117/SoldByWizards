using JetBrains.Annotations;
using SoldByWizards.Player.Interactions;
using System;
using System.Collections.Generic;
using SoldByWizards.Computers;
using SoldByWizards.Input;
using SoldByWizards.Maps;
using SoldByWizards.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoldByWizards.Items
{
    // TODO: Rewrite this class to handle multiple held items (Up to 4?)
    public class ItemsManager : MonoBehaviour, WizardInput.IInventoryActions
    {
        private const int MAX_ITEM_COUNT = 4;

        [SerializeField] private InteractionsManager _interactionsManager = null!;
        [SerializeField] private PlayerController _playerController = null!;
        [SerializeField] private ComputerController _computerController = null!;
        [SerializeField] private InputController _inputController = null!;
        [SerializeField] private TimedMapLoader _timedMapLoader = null!;

        public event Action<int, ItemSO>? OnItemSelected;
        public event Action<int, Item>? OnItemPickup;
        public event Action<int, Item>? OnItemDrop;

        private readonly ItemSO[] _heldItems = new ItemSO[MAX_ITEM_COUNT];
        private readonly Item[] _heldItemInstances = new Item[MAX_ITEM_COUNT];
        private readonly Bounds[] _heldItemBounds = new Bounds[MAX_ITEM_COUNT];
        private readonly List<Item?> _droppedItems = new();

        private int _selectedSlot = 0;

        [PublicAPI]
        public void SelectItem(int idx)
        {
            _selectedSlot = idx % MAX_ITEM_COUNT;

            OnItemSelected?.Invoke(_selectedSlot, _heldItems[_selectedSlot]);
        }

        public void DropAllItems()
        {
            for (var i = 0; i < MAX_ITEM_COUNT; i++)
            {
                DropItem(i, _playerController.transform.position);
            }
        }

        public void DropItemWithAnimation(int itemIdx)
        {
            // TODO: Portal out animation or something in the hotbar?
            DropItem(itemIdx, new Vector3(0, -999, 0f));
        }

        public int? ItemHotbarIndex(Item item)
        {
            for (int i = 0; i < MAX_ITEM_COUNT; i++)
            {
                var itemInstance = _heldItemInstances[i];
                if (itemInstance == item)
                    return i;
            }

            return null;
        }

        private void Start()
        {
            _interactionsManager.OnObjectInteract += OnObjectInteract;
            _interactionsManager.OnInteractWithWorld += OnInteractWithWorld;
            _timedMapLoader.OnTimerEnded += OnTimerEnded;

            _inputController.Input.Inventory.AddCallbacks(this);
        }

        private void OnObjectInteract(Ray _, RaycastHit raycastHit)
        {
            if (_heldItemInstances[_selectedSlot] != null || !raycastHit.transform.TryGetComponent<Item>(out var item)) return;

            var collider = raycastHit.collider;
            var rigidbody = collider.attachedRigidbody;
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            _heldItems[_selectedSlot] = item.ItemSO;
            _heldItemInstances[_selectedSlot] = item;
            _heldItemBounds[_selectedSlot] = collider.bounds;
            _heldItemInstances[_selectedSlot].gameObject.SetActive(false);

            if (_droppedItems.Contains(item))
                _droppedItems.Remove(item);

            OnItemPickup?.Invoke(_selectedSlot, item);

            if (_heldItems[(_selectedSlot + 1) % MAX_ITEM_COUNT] == null)
            {
                SelectItem(_selectedSlot + 1);
            }
        }

        private void OnInteractWithWorld(Ray ray, RaycastHit raycastHit)
        {
            if (_heldItemInstances[_selectedSlot] == null) return;

            _heldItemInstances[_selectedSlot].transform.LookAt(ray.origin, Vector3.up);
            var position = ray.GetPoint(raycastHit.distance - _heldItemBounds[_selectedSlot].extents.magnitude);

            DropItem(_selectedSlot, position);
        }

        private void DropItem(int itemIdx, Vector3 position)
        {
            _heldItems[itemIdx] = null;

            if (_heldItemInstances[itemIdx] != null)
            {
                if (!_droppedItems.Contains(_heldItemInstances[itemIdx]))
                    _droppedItems.Add(_heldItemInstances[itemIdx]);

                _heldItemInstances[itemIdx].transform.position = position;
                _heldItemInstances[itemIdx].gameObject.SetActive(true);
                _heldItemInstances[itemIdx] = null;
            }

            OnItemDrop?.Invoke(itemIdx, null);
        }

        private void OnDestroy()
        {
            _interactionsManager.OnObjectInteract -= OnObjectInteract;
            _interactionsManager.OnInteractWithWorld -= OnInteractWithWorld;
            _timedMapLoader.OnTimerEnded -= OnTimerEnded;
        }

        private void OnTimerEnded()
        {
            // Rally up all the items we have, so we can get ready to sell.
            Debug.Log("timer is over, selling...");

            List<Item> items = new();
            foreach (var item in _droppedItems)
            {
                if (item == null || item.MarkedForSale)
                    continue;

                // make our best guess as to if this mesh is real
                // this kinda sucks please fix this with a collider or something lol
                bool inBounds = item.transform.position.z < 0;
                if (inBounds)
                {
                    items.Add(item);
                }
            }

            // Count held items as items that can be sold
            foreach (var heldItem in _heldItemInstances)
            {
                if (heldItem == null || heldItem.MarkedForSale)
                    continue;

                items.Add(heldItem);
            }

            // mark items as collected in the computer
            _computerController.RegisterCollectedItems(items);
        }

        public void OnScrollInventory(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            var direction = (int)Mathf.Sign(context.ReadValue<Vector2>().y);

            if (direction == 0) return;

            SelectItem(_selectedSlot + direction);
        }

        public void OnInventorySlot1(InputAction.CallbackContext context) => SelectItem(0);

        public void OnInventorySlot2(InputAction.CallbackContext context) => SelectItem(1);

        public void OnInventorySlot3(InputAction.CallbackContext context) => SelectItem(2);

        public void OnInventorySlot4(InputAction.CallbackContext context) => SelectItem(3);
    }
}
