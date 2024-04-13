using SoldByWizards.Player.Interactions;
using UnityEngine;

namespace SoldByWizards.Items
{
    // TODO: Rewrite this class to handle multiple held items (Up to 4?)
    // TODO: Further filter by an Item component class
    public class ItemsManager : MonoBehaviour
    {
        [SerializeField] private InteractionsManager _interactionsManager;

        private GameObject _heldItem;
        private Bounds _heldItemBounds;

        private void Start()
        {
            _interactionsManager.OnObjectInteract += OnObjectInteract;
            _interactionsManager.OnInteractWithWorld += OnInteractWithWorld;
        }

        private void OnObjectInteract(Ray _, RaycastHit raycastHit)
        {
            if (_heldItem != null || !raycastHit.transform.TryGetComponent<Item>(out var item)) return;

            var collider = raycastHit.collider;
            var rigidbody = collider.attachedRigidbody;
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            _heldItem = raycastHit.transform.gameObject;
            _heldItemBounds = collider.bounds;
            _heldItem.SetActive(false);
        }

        private void OnInteractWithWorld(Ray ray, RaycastHit raycastHit)
        {
            if (_heldItem == null) return;

            var colliderBounds = raycastHit.collider.bounds;

            _heldItem.transform.position = ray.GetPoint(raycastHit.distance - _heldItemBounds.extents.magnitude);
            _heldItem.transform.LookAt(ray.origin, Vector3.up);
            _heldItem.SetActive(true);
            _heldItem = null;
        }

        private void OnDestroy()
        {
            _interactionsManager.OnObjectInteract -= OnObjectInteract;
            _interactionsManager.OnInteractWithWorld -= OnInteractWithWorld;
        }
    }
}
