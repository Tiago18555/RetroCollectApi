﻿using RetroCollect.Models;
using RetroCollectApi.Application.Shared;
using System.Text.Json.Serialization;

namespace RetroCollectApi.Application.UseCases.GameOperations.ManageRating
{
    public class EditRatingResponseModel
    {
        public Guid RatingId { get; set; }
        public InternalGame Game { get; set; }
        public InternalUser User { get; set; }
        public int RatingValue { get; set; }
        public string Review { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
