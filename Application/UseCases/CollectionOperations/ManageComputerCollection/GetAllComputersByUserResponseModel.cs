using Application.Shared;
using Domain.Enums;

namespace Application.UseCases.CollectionOperations.ManageComputerCollection;

public class GetAllComputersByUserResponseModel
{
    public Guid Id { get; set; }

    private Condition Condition { get; set; }
    public string condition => Enum.GetName(typeof(Condition), Condition);

    public DateTime PurchaseDate { get; set; }
    public string Notes { get; set; }

    private OwnershipStatus OwnershipStatus { get; set; }
    public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);
    public InternalComputer Computer { get; set; }
}
