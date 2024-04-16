using System;
using System.Collections;
using System.Collections.Generic;
using AuraTween;
using Cysharp.Threading.Tasks;
using SoldByWizards.Game;
using SoldByWizards.Input;
using SoldByWizards.Items;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoldByWizards.UI
{
    public class FailStateUIController : MonoBehaviour
    {
        private static bool firstBoot = true;

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
            Time.timeScale = 1f;

            if (firstBoot)
            {
                _canvasGroup.alpha = 1f;
                _failText.color = Color.white.WithA(0f);
                StartLore().Forget();
            }
            else
            {
                _failText.text = string.Empty;
                _tweenManager.Run(1f, 0f, _animationDuration, a => _canvasGroup.alpha = a, Easer.InOutSine);
            }

            _gameController.OnDayFailed += OnDayFailed;
        }

        private void OnDayFailed() => FailAnimation().Forget();

        private async UniTask StartLore()
        {
            Time.timeScale = 0f;

            await DisplayText("You are a wizard who needs to make rent.");
            await DisplayText("To make money, you choose to start dropshipping.");
            await DisplayText("You must summon portals, steal- I mean summon items, and sell them.");
            await DisplayText("Armed with your teleporter, you start your money making scheme...");

            Time.timeScale = 1f;
            firstBoot = false;

            await _tweenManager.Run(1f, 0f, _animationDuration, a => _canvasGroup.alpha = a, Easer.InOutSine);
        }

        private async UniTask FailAnimation()
        {
            _inputController.Disable();

            var delay = TimeSpan.FromSeconds(_darkDuration);

            await UniTask.Delay(delay, DelayType.UnscaledDeltaTime);

            await _tweenManager.Run(0f, 1f, _animationDuration, a => _canvasGroup.alpha = a, Easer.InOutSine);

            Time.timeScale = 0f;

            await UniTask.Delay(delay, DelayType.UnscaledDeltaTime);

            await DisplayText(_textStrings1);
            await UniTask.Delay(delay, DelayType.UnscaledDeltaTime);
            await DisplayText(_textStrings2);
            await UniTask.Delay(delay, DelayType.UnscaledDeltaTime);
            await DisplayText(_textStrings3);

            // Reset stock market
            StockMarket.GameTime = 0;
            StockMarket.ResetStockMarket();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private async UniTask DisplayText(string[] strings)
        {
            var rng = new System.Random();

            var randomStr = strings[rng.Next(0, strings.Length)];

            await DisplayText(randomStr);
        }

        private async UniTask DisplayText(string str)
        {
            _failText.text = str;

            await _tweenManager.Run(0f, 1f, _animationDuration, a => _failText.color = Color.white * a, Easer.InOutSine);

            await UniTask.Delay(TimeSpan.FromSeconds(_textDuration), DelayType.UnscaledDeltaTime);

            await _tweenManager.Run(1f, 0f, _animationDuration, a => _failText.color = Color.white * a, Easer.InOutSine);
        }

        private void OnDestroy()
        {
            _gameController.OnDayFailed -= OnDayFailed;
        }
    }
}
