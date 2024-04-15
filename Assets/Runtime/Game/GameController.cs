﻿using System;
using System.Collections.Generic;
using AuraTween;
using Cysharp.Threading.Tasks;
using SoldByWizards.Computers;
using SoldByWizards.Items;
using SoldByWizards.Reviews;
using SoldByWizards.Util;
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

        [SerializeField] private TweenManager _tweenManager;
        [SerializeField] private ComputerController _computerController = null!;
        [SerializeField] private ItemsManager _itemsManager = null!;
        [SerializeField] private ReviewController _reviewController = null!;
        [SerializeField] private float _baseRentAmount = 50f;
        [SerializeField] private float _rentIncreasePerCycle = 25f; // TODO: make into curve
        [SerializeField] private float _secondsPerRentCycle = 60f;
        [SerializeField] private Vector2 _sellCheckTimeRange = new Vector2(0f, 1f);
        [SerializeField] private RandomAudioPool? _chaChingAudioPool;
        [SerializeField] private GameObject _itemSellPrefab;

        public event Action OnDaySucceeded;
        public event Action OnDayFailed;
        public event Action<Item, float> OnItemSold;
        public event Action<float>? OnDayProgressUpdated;

        private int _currentDay = 0;
        private float _currentMoney;
        private float _currentRent;
        private float _timeElapsed = 0f;
        private bool _dayIsProgressing = false;
        private bool _isAlive = true;

        private void Start()
        {
            // Start at day 0
            _dayIsProgressing = true;
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
            SellAllItems(items)
                .AttachExternalCancellation(this.GetCancellationTokenOnDestroy())
                .Forget();
        }

        private async UniTask SellAllItems(List<Item> items)
        {
            UniTask[] tasks = new UniTask[items.Count];

            for (var i = 0; i < items.Count; i++)
            {
                tasks[i] = DelayThenSellItem(items[i])
                    .AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
            }

            await UniTask.WhenAll(tasks);

            // play sound
            if (_chaChingAudioPool != null)
            {
                _chaChingAudioPool.PlayRandom();
            }
        }

        private async UniTask DelayThenSellItem(Item item)
        {
            item.MarkedForSale = true;

            await UniTask.WaitForSeconds(Random.Range(_sellCheckTimeRange.x, _sellCheckTimeRange.y));

            _currentMoney += item.SellPrice;

            var hotbarIndex = _itemsManager.ItemHotbarIndex(item);
            if (hotbarIndex != null)
            {
                // play sound and show money in rent UI
                OnItemSold?.Invoke(item, item.SellPrice);

                // Create a review
                _reviewController.GenerateAndSendReview(item.ItemSO);

                // this is a hotbar item.
                _itemsManager.DeleteItem(hotbarIndex.Value);
            }
            else
            {
                await SellItemAnimation(item);
            }

            // Update stock market
            StockMarket.OnItemSold(item.ItemSO);
        }

        private async UniTask SellItemAnimation(Item item)
        {
            var itemPortal = Instantiate(_itemSellPrefab);
            var itemPortalTransform = itemPortal.transform;
            var itemPortalMaterial = itemPortal.GetComponentInChildren<MeshRenderer>().material;


            Bounds? aggregateBounds = null;
            var colliderComponents = item.GetComponentsInChildren<Collider>();
            foreach (var collider in colliderComponents)
            {
                if (!aggregateBounds.HasValue)
                {
                    aggregateBounds = collider.bounds;
                }
                else
                {
                    aggregateBounds.Value.Encapsulate(collider.bounds);
                }
            }

            itemPortalTransform.localScale = aggregateBounds.Value.extents.WithY(0.25f);
            itemPortalTransform.position = aggregateBounds.Value.center.WithY(0f);
            itemPortalTransform.eulerAngles = Vector3.zero.WithY(item.transform.eulerAngles.y);

            await _tweenManager.Run(0f, 0.75f, 1f,
                a => itemPortalMaterial.SetFloat("_Intensity", a), Easer.OutBack);

            item.SellAnimation();

            await UniTask.Delay(1000);

            // play sound and show money in rent UI
            OnItemSold?.Invoke(item, item.SellPrice);

            // Create a review
            _reviewController.GenerateAndSendReview(item.ItemSO);

            await _tweenManager.Run(0.75f, 0, 1f,
                a => itemPortalMaterial.SetFloat("_Intensity", a), Easer.InBack);

            Destroy(item.gameObject);
            Destroy(itemPortal);
        }
    }
}
