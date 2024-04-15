using System;
using AuraTween;
using Cysharp.Threading.Tasks;
using SoldByWizards.Input;
using SoldByWizards.Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SoldByWizards.UI
{
    public class PauseUIController : MonoBehaviour, WizardInput.IPauseActions
    {
        [SerializeField] private TweenManager _tweenManager;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private AudioMixer _sfxMixerGroup;
        [SerializeField] private InputController _inputController;
        [Space, SerializeField] private Transform _backgroundTransform;
        [SerializeField] private Transform _uiTransform;
        [Space, SerializeField] private Slider _sensitivitySlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [Space, SerializeField] private AudioSource _pauseSFX;
        [Space, SerializeField] private AudioSource _unpauseSFX;

        public bool Paused { get; private set; } = false;

        private CursorLockMode _cachedCursorLockMode;

        public void SetSFXVolume(float v) => _sfxMixerGroup.SetFloat("SFX Volume", LinearToDecibels(v));

        public void SetSensitivity(float s) => _playerController.Sensitivity = s;

        public void Quit() => Application.Quit();

        public void OnTogglePause(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            TogglePause();
        }

        public void TogglePause()
        {
            Paused = !Paused;

            if (Paused)
            {
                PresentPauseMenu(0.35f).Forget();
                Time.timeScale = 0f;
                _inputController.Disable(_inputController.Input.Pause.TogglePause);
                _cachedCursorLockMode = Cursor.lockState;
                Cursor.lockState = CursorLockMode.None;
                _pauseSFX.Play();
            }
            else
            {
                HidePauseMenu(0.35f).Forget();
                Time.timeScale = 1f;
                _inputController.Enable();
                Cursor.lockState = _cachedCursorLockMode;
                _unpauseSFX.Play();
            }
        }

        private void Start()
        {
            if (_sfxMixerGroup.GetFloat("SFX Volume", out var v))
            {
                _sfxVolumeSlider.SetValueWithoutNotify(DecibelsToLinear(v));
            }

            _sensitivitySlider.SetValueWithoutNotify(_playerController.Sensitivity);

            _backgroundTransform.localScale = _uiTransform.localScale = Vector3.zero;
            _inputController.Input.Pause.AddCallbacks(this);
        }

        private async UniTask PresentPauseMenu(float duration)
        {
            // Background image
            _tweenManager.Run(0f, 1, duration, t => _backgroundTransform.localScale = t * Vector3.one, Easer.OutBack);

            await UniTask.Delay(TimeSpan.FromSeconds(0.2f), DelayType.UnscaledDeltaTime);

            // Settings panel
            _tweenManager.Run(0, 1, duration, t => _uiTransform.localScale = t * Vector3.one, Easer.OutBack);
        }

        private UniTask HidePauseMenu(float duration)
        {
            // Settings panel
            _tweenManager.Run(1, 0, duration + 0.2f, t => _uiTransform.localScale = t * Vector3.one, Easer.InBack);

            // Background image
            _tweenManager.Run(1f, 0, duration, t => _backgroundTransform.localScale = t * Vector3.one, Easer.InBack);

            return UniTask.CompletedTask;
        }

        private static float DecibelsToLinear(float decibels) => float.IsInfinity(decibels)
            ? 0.0f
            : Mathf.Pow(10.0f, decibels / 20.0f);

        private static float LinearToDecibels(float linear) {
            var decibels = 20.0f * Mathf.Log10(linear);

            return float.IsInfinity(decibels)
                ? -80.0f
                : decibels;
        }
    }
}
