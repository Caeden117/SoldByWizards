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
        private const float _delay = 5f;
        private const float _duration = 1f;

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
            _tweenManager.Run(1f, 0f, _duration, a => _canvasGroup.alpha = a, Easer.InOutSine);

            _gameController.OnDayFailed += OnDayFailed;
        }

        private void OnDayFailed() => FailAnimation().Forget();

        private async UniTask FailAnimation()
        {
            _inputController.Disable();

            var halfDelay = TimeSpan.FromSeconds(_delay / 2);

            await UniTask.Delay(halfDelay);

            await _tweenManager.Run(0f, 1f, _duration, a => _canvasGroup.alpha = a, Easer.InOutSine);

            await UniTask.Delay(halfDelay);

            await DisplayText(_textStrings1);
            await UniTask.Delay(halfDelay);
            await DisplayText(_textStrings2);
            await UniTask.Delay(halfDelay);
            await DisplayText(_textStrings3);

            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private async UniTask DisplayText(string[] strings)
        {
            var rng = new System.Random();

            var randomStr = strings[rng.Next(0, strings.Length)];

            _failText.text = randomStr;

            await _tweenManager.Run(0f, 1f, _duration, a => _failText.color = Color.white * a, Easer.InOutSine);

            await UniTask.Delay(TimeSpan.FromSeconds(_delay));

            await _tweenManager.Run(1f, 0f, _duration, a => _failText.color = Color.white * a, Easer.InOutSine);
        }

        private void OnDestroy()
        {
            _gameController.OnDayFailed -= OnDayFailed;
        }
    }
}
