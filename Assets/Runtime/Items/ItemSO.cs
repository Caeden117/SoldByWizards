using UnityEngine;

namespace SoldByWizards.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemSO : ScriptableObject
    {
        public GameObject? ItemPrefab;

        [Header("Shop Metadata")] public string ItemName = "New Item";
        public string ItemDescription = "Item description goes here.";
        public Sprite? ItemIcon;

        [Header("Basic Item Stats")] public float BaseSaleValue = 0.0f;

        [Header("Stock Market")] public float StockMarketLowModifier = 0.3f;
        public float StockMarketHighModifier = 3.0f;
        public float MultiSalesPenalty = -0.03f;

        public bool TwoHanded = false;
    }
}
