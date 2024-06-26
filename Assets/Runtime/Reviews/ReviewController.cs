﻿using System;
using System.Collections.Generic;
using SoldByWizards.Items;
using UnityEngine;

namespace SoldByWizards.Reviews
{
    public class ReviewController : MonoBehaviour
    {
        public event Action<GeneratedReview>? OnReviewGenerated;

        public List<Review> GenericReviews = new();
        private System.Random _random = new();

        public GeneratedReview GenerateReview(ItemSO item)
        {
            // pick a random review
            // TODO: Per item reviews
            var review = GenericReviews[_random.Next(0, GenericReviews.Count)];

            // 66% chance for item review, adjust if needed
            bool useItemReviews = (item.Reviews.Count > 0 ? _random.Next(0, 3) : 0) == 2;
            if (useItemReviews)
            {
                var index = _random.Next(0, item.Reviews.Count);
                review = item.Reviews[index];
            }

            // pick a random star value
            var starValue = _random.Next(review.PossibleStarReviews.x, review.PossibleStarReviews.y + 1);
            // get a random title/desc
            var title = review.PossibleTitles[_random.Next(0, review.PossibleTitles.Count)];
            var description = review.PossibleDescriptions[_random.Next(0, review.PossibleDescriptions.Count)];

            return new GeneratedReview(starValue, title, description, item);
        }

        public void GenerateAndSendReview(ItemSO item)
        {
            var review = GenerateReview(item);
            OnReviewGenerated?.Invoke(review);
        }
    }
}
