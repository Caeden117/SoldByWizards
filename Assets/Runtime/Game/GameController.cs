using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SoldByWizards.Computers;
using SoldByWizards.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoldByWizards.Game
{
    // Controls rent, money, and selling
    public class GameController : MonoBehaviour
    {
        public float SecondsPerRentCycle => _secondsPerRentCycle;

        public float CurrentMoney => _currentMoney;

        public float CurrentRent => GetRentForCurrentDay();

        [SerializeField] private ComputerController _computerController = null!;
        [SerializeField] private ItemsManager _itemsManager = null!;
        [SerializeField] private float _baseRentAmount = 50f;
        [SerializeField] private float _rentIncreasePerCycle = 25f; // TODO: make into curve
        [SerializeField] private float _secondsPerRentCycle = 60f;
        [SerializeField] private Vector2 _sellCheckTimeRange = new Vector2(0f, 1f);

        public event Action OnDaySucceeded;
        public event Action OnDayFailed;
        public event Action<Item, float> OnItemSold;
        public event Action<float>? OnDayProgressUpdated;

        private int _currentDay = 0;
        private float _currentMoney;
        private float _currentRent;
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
                OnDayFailed?.Invoke();
            }
            else
            {
                // progress to next day
                _timeElapsed = 0f;
                _dayIsProgressing = true;
                _currentDay += 1;

                // Reset stock market penalties
                StockMarket.ResetStockMarket();

                // minus rent from money
                _currentMoney -= rent;

                OnDaySucceeded?.Invoke();
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
                await UniTask.WaitForSeconds(Random.Range(_sellCheckTimeRange.x, _sellCheckTimeRange.y));

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
            _currentMoney += item.SellPrice;

            OnItemSold?.Invoke(item, item.SellPrice);

            var hotbarIndex = _itemsManager.ItemHotbarIndex(item);

            if (hotbarIndex != null)
            {
                // this is a hotbar item.
                _itemsManager.DropItemWithAnimation(hotbarIndex.Value);
            }
            else
            {
                // this is a dropped item.
                item.SellAnimation();
            }

            // Update stock market
            StockMarket.OnItemSold(item.ItemSO);
        }
    }
}
