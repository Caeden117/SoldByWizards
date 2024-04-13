using JetBrains.Annotations;
using SoldByWizards.Player.Interactions;
using System;
using SoldByWizards.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoldByWizards.Items
{
    // TODO: Rewrite this class to handle multiple held items (Up to 4?)
    public class ItemsManager : MonoBehaviour, WizardInput.IInventoryActions
    {
        private const int MAX_ITEM_COUNT = 4;

        [SerializeField] private InteractionsManager _interactionsManager;
        [SerializeField] private InputController _inputController;

        public event Action<int, ItemSO> OnItemSelected;
        public event Action<int, ItemSO> OnItemPickup;
        public event Action<int, ItemSO> OnItemDrop;

        private readonly ItemSO[] _heldItems = new ItemSO[MAX_ITEM_COUNT];
        private readonly GameObject[] _heldItemInstances = new GameObject[MAX_ITEM_COUNT];
        private readonly Bounds[] _heldItemBounds = new Bounds[MAX_ITEM_COUNT];

        private int _selectedSlot = 0;

        [PublicAPI]
        public void SelectItem(int idx)
        {
            _selectedSlot = idx % MAX_ITEM_COUNT;

            OnItemSelected?.Invoke(_selectedSlot, _heldItems[_selectedSlot]);
        }

        private void Start()
        {
            _interactionsManager.OnObjectInteract += OnObjectInteract;
            _interactionsManager.OnInteractWithWorld += OnInteractWithWorld;

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
            _heldItemInstances[_selectedSlot] = raycastHit.transform.gameObject;
            _heldItemBounds[_selectedSlot] = collider.bounds;
            _heldItemInstances[_selectedSlot].SetActive(false);

            OnItemPickup?.Invoke(_selectedSlot, item.ItemSO);
        }

        private void OnInteractWithWorld(Ray ray, RaycastHit raycastHit)
        {
            if (_heldItemInstances[_selectedSlot] == null) return;

            var colliderBounds = raycastHit.collider.bounds;

            _heldItems[_selectedSlot] = null;

            _heldItemInstances[_selectedSlot].transform.position = ray.GetPoint(raycastHit.distance - _heldItemBounds[_selectedSlot].extents.magnitude);
            _heldItemInstances[_selectedSlot].transform.LookAt(ray.origin, Vector3.up);
            _heldItemInstances[_selectedSlot].SetActive(true);
            _heldItemInstances[_selectedSlot] = null;

            OnItemDrop?.Invoke(_selectedSlot, null);
        }

        private void OnDestroy()
        {
            _interactionsManager.OnObjectInteract -= OnObjectInteract;
            _interactionsManager.OnInteractWithWorld -= OnInteractWithWorld;
        }

        public void OnScrollInventory(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            var direction = (int)Mathf.Sign(context.ReadValue<float>());

            if (direction == 0) return;

            SelectItem(_selectedSlot + direction);
        }

        public void OnInventorySlot1(InputAction.CallbackContext context) => SelectItem(0);

        public void OnInventorySlot2(InputAction.CallbackContext context) => SelectItem(1);

        public void OnInventorySlot3(InputAction.CallbackContext context) => SelectItem(2);

        public void OnInventorySlot4(InputAction.CallbackContext context) => SelectItem(3);
    }
}
