using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldByWizards.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField, Header("Basic Item Stats")] private string _itemName = "New Item";
        [SerializeField] private float _baseSaleValue = 0.0f;

        [SerializeField, Header("Stock Market")] private float _stockMarketLowModifier = 0.3f;
        [SerializeField] private float _stockMarketHighModifier = 3.0f;
        [SerializeField] private float _multiSalesPenalty = -0.03f;

        [SerializeField] private bool _twoHanded = false;
    }
}
