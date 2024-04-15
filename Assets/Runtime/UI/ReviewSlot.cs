using System.Collections.Generic;
using SoldByWizards.Reviews;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoldByWizards.UI
{
    public class ReviewSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText = null!;
        [SerializeField] private TextMeshProUGUI _descriptionText = null!;
        [SerializeField] private ReviewStar[] _stars = new ReviewStar[5];         // should always have 5 entries
        [SerializeField] private TextMeshProUGUI _itemNameText = null!;
        [SerializeField] private Image _itemIcon = null!;

        public void SetReview(GeneratedReview generatedReview)
        {
            _titleText.text = generatedReview.Title;
            _descriptionText.text = generatedReview.Description;

            // set stars to appropriate rating
            for (int i = 0; i < 5; i++)
            {
                _stars[i].SetFilled(generatedReview.StarRating >= i + 1);
            }

            _itemNameText.text = generatedReview.Item.ItemName;
            _itemIcon.sprite = generatedReview.Item.ItemIcon;
        }
    }
}
