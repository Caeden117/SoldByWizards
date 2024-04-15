using System;
using SoldByWizards.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoldByWizards.Player.Interactions
{
    public class InteractionsManager : MonoBehaviour, WizardInput.IInteractionsActions
    {
        [SerializeField] private Camera _raycastCamera;
        [SerializeField] private InputController _inputController;
        [Space, SerializeField] private LayerMask _interactionLayerMask;
        [SerializeField] private float _interactRange;
        [SerializeField] private LayerMask _generalLayerMask;

        public event Action<Ray, RaycastHit> OnObjectInteract;
        public event Action<Ray, RaycastHit> OnButtonInteract;
        public event Action<Ray, RaycastHit> OnWorldInteract;
        public event Action OnInteractKeyPressed;
        public event Action OnInteractionHover;
        public event Action OnInteractionUnhover;

        private bool _hovering;

        private void Start() => _inputController.Input.Interactions.AddCallbacks(this);

        private void Update()
        {
            var middlePoint = 0.5f * new Vector2(_raycastCamera.scaledPixelWidth, _raycastCamera.scaledPixelHeight);
            var ray = _raycastCamera.ScreenPointToRay(middlePoint);

            var hovering = Physics.SphereCast(ray, 0.25f, out _, _interactRange, _interactionLayerMask);

            if (hovering == _hovering) return;

            (hovering ? OnInteractionHover : OnInteractionUnhover)?.Invoke();

            _hovering = hovering;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            var middlePoint = 0.5f * new Vector2(_raycastCamera.scaledPixelWidth, _raycastCamera.scaledPixelHeight);
            var ray = _raycastCamera.ScreenPointToRay(middlePoint);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _interactRange, _interactionLayerMask)
                || Physics.SphereCast(ray, 0.25f, out hit, _interactRange, _interactionLayerMask))
            {
                OnObjectInteract?.Invoke(ray, hit);
                OnInteractKeyPressed?.Invoke();
                return;
            }

            OnInteractKeyPressed?.Invoke();
        }

        public void OnInteractWithButton(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            var middlePoint = 0.5f * new Vector2(_raycastCamera.scaledPixelWidth, _raycastCamera.scaledPixelHeight);
            var ray = _raycastCamera.ScreenPointToRay(middlePoint);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _interactRange, _interactionLayerMask)
                || Physics.SphereCast(ray, 0.25f, out hit, _interactRange, _interactionLayerMask))
            {
                OnButtonInteract?.Invoke(ray, hit);
                return;
            }
        }

        public void OnInteractWithWorld(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            var middlePoint = 0.5f * new Vector2(_raycastCamera.scaledPixelWidth, _raycastCamera.scaledPixelHeight);
            var ray = _raycastCamera.ScreenPointToRay(middlePoint);

            RaycastHit worldHit;
            if (Physics.Raycast(ray, out worldHit, _interactRange, _generalLayerMask)
                || Physics.SphereCast(ray, 0.15f, out worldHit, _interactRange, _generalLayerMask))
            {
                OnWorldInteract?.Invoke(ray, worldHit);
                return;
            }

            worldHit.distance = _interactRange * 0.5f;
            worldHit.point = ray.origin + ray.direction * worldHit.distance;
            worldHit.normal = Vector3.up;
            OnWorldInteract?.Invoke(ray, worldHit);
        }
    }
}
