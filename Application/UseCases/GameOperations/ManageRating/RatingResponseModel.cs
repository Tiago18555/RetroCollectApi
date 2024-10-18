namespace Application.UseCases.GameOperations.ManageRating;

public class RatingResponseModel
{
    public int GameId { get; set; }
    public int RatingValue { get; set; }
    public string Review { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
