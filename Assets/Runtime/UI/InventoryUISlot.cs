using System;
using System.Collections;
using System.Collections.Generic;
using AuraTween;
using JetBrains.Annotations;
using SoldByWizards.Items;
using SoldByWizards.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoldByWizards.UI
{
    public class InventoryUISlot : MonoBehaviour
    {
        private const float _animTime = 0.1f;

        [SerializeField] private TweenManager _tweenManager;
        [Space, SerializeField] private RectTransform _backgroundTransform;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TextMeshProUGUI _salePriceText;
        [SerializeField] private Image _itemImage;

        private Tween? _colorTween;
        private Tween? _backgroundTween;
        private ItemSO _cachedItem;

        [PublicAPI]
        public void SetSelected(bool selected)
        {
            var startColor = !selected ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.5f);
            var endColor = selected ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.5f);

            _colorTween?.Cancel();
            _colorTween = _tweenManager.Run(startColor, endColor, _animTime,
                color => _backgroundImage.color = color, Easer.InOutSine);
        }

        [PublicAPI]
        public void SetItem(Item item)
        {
            if (item?.ItemSO != _cachedItem)
            {
                _cachedItem = item?.ItemSO;
                var startBottom = !item ? -25f : 0;
                var endBottom = item ? -25f : 0;

                _backgroundTween?.Cancel();
                _backgroundTween = _tweenManager.Run(startBottom, endBottom, _animTime,
                    b => _backgroundTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, b, 100f - b),
                    Easer.InOutSine);
            }

            var itemSprite = item ? item.ItemSO.ItemIcon : null;
            _itemImage.sprite = itemSprite;
            _itemImage.enabled = item;

            var text = item ? $"${item.SellPrice:N2}" : string.Empty;
            _salePriceText.text = text;
        }
    }
}
