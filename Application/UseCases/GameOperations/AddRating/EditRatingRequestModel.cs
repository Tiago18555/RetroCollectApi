namespace RetroCollectApi.Application.UseCases.GameOperations.AddRating
{
    public class EditRatingRequestModel
    {
        public Guid RatingId { get; set; }
        public int RatingValue { get; set; }
        public string Review { get; set; }
    }
}
