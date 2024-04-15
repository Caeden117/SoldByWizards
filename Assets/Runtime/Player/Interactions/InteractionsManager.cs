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
        public event Action<Ray, RaycastHit> OnInteractWithWorld;
        public event Action OnInteractKeyPressed;

        private void Start() => _inputController.Input.Interactions.AddCallbacks(this);

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            var middlePoint = 0.5f * new Vector2(_raycastCamera.scaledPixelWidth, _raycastCamera.scaledPixelHeight);
            var ray = _raycastCamera.ScreenPointToRay(middlePoint);

            if (Physics.SphereCast(ray, 0.15f, out var hit, _interactRange, _interactionLayerMask))
            {
                OnObjectInteract?.Invoke(ray, hit);
                OnInteractKeyPressed?.Invoke();
                return;
            }

            if (Physics.SphereCast(ray, 0.15f, out var worldHit, _interactRange, _generalLayerMask))
            {
                OnInteractWithWorld?.Invoke(ray, worldHit);
            }

            OnInteractKeyPressed?.Invoke();
        }
    }
}
