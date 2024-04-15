using System;
using SoldByWizards.Items;

namespace SoldByWizards.Reviews
{
    [Serializable]
    public class GeneratedReview
    {
        public int StarRating;
        public string Title;
        public string Description;
        public ItemSO Item;

        public GeneratedReview(int starRating, string title, string description, ItemSO item)
        {
            StarRating = starRating;
            Title = title;
            Description = description;
            Item = item;
        }
    }
}
