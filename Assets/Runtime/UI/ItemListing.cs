using SoldByWizards.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoldByWizards
{
    public class ItemListing : MonoBehaviour
    {
        public const string SellerName = "Sold by: Wizards";

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
    }
}
