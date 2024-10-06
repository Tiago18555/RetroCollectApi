﻿using Application.Shared;
using Domain.Enums;

namespace Application.UseCases.UserCollectionOperations.AddItems
{
    public partial class ManageGameCollectionUsecase
    {
        public class GetAllCollectionsByUserResponseModel
        {
            public Guid UserCollectionId { get; set; }

            private Condition Condition { get; set; }
            public string condition => Enum.GetName(typeof(Condition), Condition);

            public DateTime PurchaseDate { get; set; }
            public string Notes { get; set; }

            private OwnershipStatus OwnershipStatus { get; set; }
            public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);
            public InternalGame Game { get; set; }
            public int ComputerId { get; set; }
            public int ConsoleId { get; set; }
        }
    }
}
