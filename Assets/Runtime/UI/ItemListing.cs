using System;
using SoldByWizards.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoldByWizards
{
    public class ItemListing : MonoBehaviour
    {
        public const string SellerName = "Sold by: Wizards";

        public bool Saturated = false; // when the text is all spammed out, we're saturated

        [SerializeField] private TextMeshProUGUI? _titleText;
        [SerializeField] private TextMeshProUGUI? _sellerText;
        [SerializeField] private TextMeshProUGUI? _descriptionText;
        [SerializeField] private Image? _iconImage;

        private ItemSO? _itemSo;
        public void SetItem(ItemSO itemSo)
        {
            _itemSo = itemSo;
            if (_iconImage != null)
            {
                _iconImage.sprite = itemSo.ItemIcon;
            }
        }

        public void Clear()
        {
            Saturated = false;
            if (_titleText != null)
            {
                _titleText.text = "";
            }
            if (_sellerText != null)
            {
                _sellerText.text = "";
            }
            if (_descriptionText != null)
            {
                _descriptionText.text = "";
            }
        }

        public int GetTotalTextLength()
        {
            if (_itemSo == null)
                return 0;

            return _itemSo.ItemName.Length + SellerName.Length + _itemSo.ItemDescription.Length;
        }

        public void SetTextCount(int textCount)
        {
            if (Saturated || _itemSo == null || _titleText == null || _sellerText == null || _descriptionText == null)
                return;

            int nameLength = _itemSo.ItemName.Length;
            int sellerNameLength = SellerName.Length;
            int descriptionLength = _itemSo.ItemDescription.Length;

            _titleText.text = GetTextFromCount(_itemSo.ItemName, 0, nameLength, textCount);
            _sellerText.text = GetTextFromCount(SellerName, nameLength, sellerNameLength, textCount);
            _descriptionText.text = GetTextFromCount(_itemSo.ItemDescription, nameLength + sellerNameLength, descriptionLength, textCount);

            if (textCount >= nameLength + sellerNameLength + descriptionLength)
                Saturated = true;
        }

        private string GetTextFromCount(string fullText, int startIndex, int length, int count)
        {
            if (startIndex > count)
            {
                return string.Empty;
            }

            if (startIndex + length <= count)
            {
                return fullText;
            }

            int amount = count - startIndex;
            return fullText.Substring(0, amount);
        }
    }
}
