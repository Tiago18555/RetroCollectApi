﻿using Application.Shared;
using Domain.Enums;

namespace Application.UseCases.UserCollectionOperations.ManageComputerCollection;

public class UpdateComputerResponseModel
{
    public Guid UserCollectionId { get; set; }

    private Condition Condition { get; set; }
    public string condition => Enum.GetName(typeof(Condition), Condition);

    public DateTime PurchaseDate { get; set; }
    public string Notes { get; set; }

    private OwnershipStatus OwnershipStatus { get; set; }
    public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);

    public InternalUser User { get; set; }
    public InternalComputer Computer {  get; set; }
}
