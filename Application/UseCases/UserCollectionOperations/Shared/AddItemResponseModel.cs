﻿using RetroCollect.Models;
using RetroCollectApi.CrossCutting.Enums.ForModels;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared
{
    public class AddItemResponseModel
    {
        public Guid UserCollectionId { get; set; }

        private Condition Condition { get; set; }
        public string condition => Enum.GetName(typeof(Condition), Condition);

        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }

        private OwnershipStatus OwnershipStatus { get; set; }
        public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
