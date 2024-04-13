using SoldByWizards.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoldByWizards
{
    public class ItemListing : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI? _titleText;
        [SerializeField] private TextMeshProUGUI? _sellerText;
        [SerializeField] private TextMeshProUGUI? _descriptionText;
        [SerializeField] private Image? _iconImage;

        public void SetItem(ItemSO itemSo)
        {
            if (_iconImage != null)
            {
                _iconImage.sprite = itemSo.ItemIcon;
            }

            // TODO: Spam typing bit
        }
    }
}
