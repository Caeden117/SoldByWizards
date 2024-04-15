using System.Collections.Generic;
using SoldByWizards.Items;
using UnityEngine;

namespace SoldByWizards.Reviews
{
    public class ReviewController : MonoBehaviour
    {
        public List<Review> GenericReviews = new();
        private System.Random _random = new();

        public GeneratedReview GenerateReview(ItemSO item)
        {
            // pick a random review
            // TODO: Per item reviews
            var review = GenericReviews[_random.Next(0, GenericReviews.Count)];

            // pick a random star value
            var starValue = _random.Next(review.PossibleStarReviews.x, review.PossibleStarReviews.y + 1);
            // get a random title/desc
            var title = review.PossibleTitles[_random.Next(0, review.PossibleTitles.Count)];
            var description = review.PossibleDescriptions[_random.Next(0, review.PossibleDescriptions.Count)];

            return new GeneratedReview(starValue, title, description, item);
        }
    }
}
