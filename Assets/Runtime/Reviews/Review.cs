using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace SoldByWizards.Reviews
{
    [Serializable]
    public class Review
    {
        public Vector2Int PossibleStarReviews = new(1, 1);
        public List<string> PossibleTitles = new();
        [TextArea(5,100)] public List<string> PossibleDescriptions = new();
    }
}
