﻿using AuraTween;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using SoldByWizards.Input;
using SoldByWizards.Player;
using SoldByWizards.Player.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoldByWizards.Computers
{
    // handles tabbing in/out of the computer
    public class ComputersManager : MonoBehaviour, WizardInput.IComputerActions
    {
        [SerializeField] private InteractionsManager _interactionsManager = null!;
        [SerializeField] private InputController _inputController = null!;
        [SerializeField] private PlayerController _playerController = null!;
        [SerializeField] private TweenManager _tweenManager = null!;
        [SerializeField] private ComputerController _computerController = null!;
        [SerializeField] private ListingPageSpamController _listingPageSpamController = null!;

        private Computer? _activeComputer;

        [SerializeField] private Ease _animationEase = Ease.Linear;
        [SerializeField] private float _animationLength = 1f;

        // used for computer view animation transition
        private bool _transitioning = false;

        private void Start()
        {
            _interactionsManager.OnObjectInteract += OnObjectInteract;
            _inputController.Input.Computer.AddCallbacks(this);
        }

        private void OnDestroy()
        {
            _interactionsManager.OnObjectInteract -= OnObjectInteract;
            _inputController.Input.Computer.RemoveCallbacks(this);
        }

        private void OnInteractKeyPressed()
        {
            if (_activeComputer == null || _transitioning || _listingPageSpamController.CurrentlyTyping)
                return;

            var computer = _activeComputer;

            // disable computer
            Debug.Log("Disabling computer!");
            _activeComputer = null;
            ComputerDisableAnimation(computer).Forget();
            _inputController.EnablePlayerInput(PlayerInputDisableSource.UsingComputer);
        }

        private void OnObjectInteract(Ray _, RaycastHit raycastHit)
        {
            if (_transitioning || !raycastHit.transform.TryGetComponent<Computer>(out var computer)) return;

            if (_activeComputer != null)
                return;

            // enable computer
            Debug.Log("Using computer!");
            _activeComputer = computer;
            _inputController.DisablePlayerInput(PlayerInputDisableSource.UsingComputer);
            ComputerEnableAnimation(computer).Forget();
        }

        private async UniTask ComputerEnableAnimation(Computer computer)
        {
            _transitioning = true;
            computer.ComputerCamera.enabled = true;
            _computerController.ComputerSelected(computer);

            // lerp player camera to static computer spot
            await LerpCamValues(
                computer.ComputerCamera,
                _playerController.Camera.transform.position,
                computer.CameraFocusPoint!.position,
                _playerController.Camera.transform.rotation,
                computer.CameraFocusPoint!.rotation,
                _playerController.Camera.fieldOfView,
                computer.IntendedFov
            );

            computer.CustomVisualsWhenEnabled.SetActive(true);
            // _computerItemEntryController.CreateListings(computer);
            _transitioning = false;
        }

        private async UniTask ComputerDisableAnimation(Computer computer)
        {
            _transitioning = true;
            computer.CustomVisualsWhenEnabled.SetActive(false);
            _computerController.ComputerDeselected(computer);

            // lerp static computer spot to player camera
            await LerpCamValues(
                computer.ComputerCamera,
                computer.CameraFocusPoint!.position,
                _playerController.Camera.transform.position,
                computer.CameraFocusPoint!.rotation,
                _playerController.Camera.transform.rotation,
                computer.IntendedFov,
                _playerController.Camera.fieldOfView
            );

            computer.ComputerCamera.enabled = false;
            _transitioning = false;
        }

        private async UniTask LerpCamValues(Camera camera, Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation, float startFov, float endFov)
        {
            await _tweenManager.Run(0f, 1f, _animationLength, (value) =>
            {
                camera.transform.position = Vector3.Lerp(startPosition, endPosition, value);
                camera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, value);
                camera.fieldOfView = Mathf.Lerp(startFov, endFov, value);
            }, _animationEase.ToProcedure());
        }

        public void OnToggleComputer(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            OnInteractKeyPressed();
        }
    }
}
