using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldByWizards.Items
{
    public static class StockMarket
    {
        public static float GameTime = 0;

        private static Dictionary<ItemSO, int> _timesSold = new();

        public static float CalculatePriceFor(ItemSO item)
        {
            var baseSale = item.BaseSaleValue;

            // TODO: TAKE DAYS INTO ACCOUNT FOR STOCK MARKET CALCULATIONS
            var stockMarketNormalizedMultiplier = Mathf.PerlinNoise(GameTime, item.StockMarketSeed);

            var stockMarketMultiplier = Remap(stockMarketNormalizedMultiplier, 0, 1,
                item.StockMarketLowModifier, item.StockMarketHighModifier);

            var penalty = _timesSold.TryGetValue(item, out var timesSold)
                ? timesSold * item.MultiSalesPenalty
                : 0.0f;

            return baseSale * (stockMarketMultiplier + penalty);
        }

        public static void OnItemSold(ItemSO item)
        {
            if (!_timesSold.TryAdd(item, 1))
                _timesSold[item]++;
        }

        public static void ResetStockMarket() => _timesSold.Clear();

        public static void DesaturateStockMarket()
        {
            foreach (var kvp in _timesSold)
            {
                _timesSold[kvp.Key]--;
            }
        }

        private static float Remap(this float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
