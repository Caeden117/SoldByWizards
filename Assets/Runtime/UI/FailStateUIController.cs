using System;
using System.Collections;
using System.Collections.Generic;
using AuraTween;
using Cysharp.Threading.Tasks;
using SoldByWizards.Game;
using SoldByWizards.Input;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoldByWizards
{
    public class FailStateUIController : MonoBehaviour
    {
        [SerializeField] private float _textDuration = 5f;
        [SerializeField] private float _animationDuration = 1f;
        [SerializeField] private float _darkDuration = 1f;

        [SerializeField] private TweenManager _tweenManager;
        [SerializeField] private InputController _inputController;
        [SerializeField] private GameController _gameController;
        [Space, SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _failText;
        [Space, SerializeField] private string[] _textStrings1;
        [Space, SerializeField] private string[] _textStrings2;
        [Space, SerializeField] private string[] _textStrings3;

        private void Start()
        {
            _failText.text = string.Empty;
            _tweenManager.Run(1f, 0f, _animationDuration, a => _canvasGroup.alpha = a, Easer.InOutSine);

            _gameController.OnDayFailed += OnDayFailed;
        }

        private void OnDayFailed() => FailAnimation().Forget();

        private async UniTask FailAnimation()
        {
            _inputController.Disable();

            var delay = TimeSpan.FromSeconds(_darkDuration);

            await UniTask.Delay(delay);

            await _tweenManager.Run(0f, 1f, _animationDuration, a => _canvasGroup.alpha = a, Easer.InOutSine);

            await UniTask.Delay(delay);

            await DisplayText(_textStrings1);
            await UniTask.Delay(delay);
            await DisplayText(_textStrings2);
            await UniTask.Delay(delay);
            await DisplayText(_textStrings3);

            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private async UniTask DisplayText(string[] strings)
        {
            var rng = new System.Random();

            var randomStr = strings[rng.Next(0, strings.Length)];

            _failText.text = randomStr;

            await _tweenManager.Run(0f, 1f, _animationDuration, a => _failText.color = Color.white * a, Easer.InOutSine);

            await UniTask.Delay(TimeSpan.FromSeconds(_textDuration));

            await _tweenManager.Run(1f, 0f, _animationDuration, a => _failText.color = Color.white * a, Easer.InOutSine);
        }

        private void OnDestroy()
        {
            _gameController.OnDayFailed -= OnDayFailed;
        }
    }
}
