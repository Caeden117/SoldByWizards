﻿using System.Collections.Generic;
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
    }
}
