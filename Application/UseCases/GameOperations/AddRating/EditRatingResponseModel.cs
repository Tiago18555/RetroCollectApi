namespace RetroCollectApi.Application.UseCases.GameOperations.AddRating
{
    public class EditRatingResponseModel
    {
        public Guid RatingId { get; set; }
        public Guid UserId { get; set; }
        public int GameId { get; set; }
        public int RatingValue { get; set; }
        public string Review { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
