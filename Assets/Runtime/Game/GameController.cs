using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SoldByWizards.Computers;
using SoldByWizards.Items;
using SoldByWizards.Maps;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoldByWizards.Game
{
    // Controls rent, money, and selling
    public class GameController : MonoBehaviour
    {
        public float SecondsPerRentCycle => _secondsPerRentCycle;

        [SerializeField] private ComputerController _computerController = null!;
        [SerializeField] private float _baseRentAmount = 50f;
        [SerializeField] private float _rentIncreasePerCycle = 25f; // TODO: make into curve
        [SerializeField] private float _secondsPerRentCycle = 60f;

        public Action<float>? OnDayProgressUpdated;

        private int _currentDay = 0;
        private float _currentMoney;
        private float _timeElapsed = 0f;
        private bool _dayIsProgressing = false;
        private List<Item> _itemsToSell = new();
        private bool _isAlive = true;

        private void Start()
        {
            // Start at day 0
            _dayIsProgressing = true;

            SellLoop().AttachExternalCancellation(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private void Update()
        {
            if (!_dayIsProgressing)
                return;

            _timeElapsed += Time.deltaTime;
            OnDayProgressUpdated?.Invoke(_timeElapsed / _secondsPerRentCycle);
            if (_timeElapsed > _secondsPerRentCycle)
            {
                _dayIsProgressing = false;
                CheckForNextDay();
            }
        }

        private void CheckForNextDay()
        {
            var rent = GetRentForCurrentDay();
            if (_currentMoney < rent)
            {
                // fail player, game over
                Debug.LogError("YOU FREAKIN' DIED!");
            }
            else
            {
                // progress to next day
                _timeElapsed = 0f;
                _dayIsProgressing = true;
                _currentDay += 1;
            }
        }

        private float GetRentForCurrentDay()
        {
            return _baseRentAmount + (_currentDay * _rentIncreasePerCycle);
        }

        private void OnEnable()
        {
            _computerController.OnItemsListedForSale += OnItemsListedForSell;
        }

        private void OnDisable()
        {
            _computerController.OnItemsListedForSale -= OnItemsListedForSell;
        }

        private void OnItemsListedForSell(List<Item> items)
        {
            Debug.Log("Marking items for The Great Sell!");
            Debug.Log(items.Count);
            foreach (var item in items)
            {
                if (item == null)
                    continue;

                item.MarkedForSale = true;
                _itemsToSell.Add(item);
            }
        }

        private async UniTask SellLoop()
        {
            while (_isAlive)
            {
                await UniTask.Delay(500);

                if (_itemsToSell.Count == 0)
                    continue;

                // pick a random item to sell
                var itemToSell = _itemsToSell[Random.Range(0, _itemsToSell.Count)];
                _itemsToSell.Remove(itemToSell);
                SellItem(itemToSell);
            }
        }

        private void SellItem(Item item)
        {
            Debug.Log("ITEM IS BEING SOLD!");
            _currentMoney += item.SellPrice;
            item.SellAnimation();
        }
    }
}
