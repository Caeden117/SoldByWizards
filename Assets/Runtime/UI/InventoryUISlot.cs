﻿using System;
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
        [SerializeField] private GameObject _warningFlash;

        private Color _originalBackgroundColor;
        private Color _originalTextColor;

        private Tween? _colorTween;
        private Tween? _textTween;
        private Tween? _backgroundTween;
        private ItemSO _cachedItem;

        private void Start()
        {
            _originalBackgroundColor = _backgroundImage.color;
            _originalTextColor = _salePriceText.color;
        }

        [PublicAPI]
        public void SetSelected(bool selected)
        {
            var startColor = !selected ? Color.white : _originalBackgroundColor;
            var endColor = selected ? Color.white : _originalBackgroundColor;

            _colorTween?.Cancel();
            _colorTween = _tweenManager.Run(startColor, endColor, _animTime,
                color => _backgroundImage.color = color, Easer.InOutSine);

            var startColor2 = !selected ? Color.black : _originalTextColor;
            var endColor2 = selected ? Color.black : _originalTextColor;

            _textTween?.Cancel();
            _textTween = _tweenManager.Run(startColor2, endColor2, _animTime,
                color => _salePriceText.color = color, Easer.InOutSine);
        }

        [PublicAPI]
        public void SetItem(Item item)
        {
            _warningFlash.SetActive(item);

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
