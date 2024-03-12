namespace RetroCollectApi.Application.UseCases.GameOperations.AddRating
{
    public class AddRatingRequestModel
    {
        public Guid UserId { get; set; }
        public int GameId { get; set; }
        public int RatingValue { get; set; }
        public string Review { get; set; }
    }
}
