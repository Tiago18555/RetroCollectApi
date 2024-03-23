namespace Application.UseCases.UserWishlistOperations.GameOperations
{
    public class AddToUserWishlistResponseModel
    {
        public int GameId { get; set; }
        public Guid UserId { get; set; }
    }
}
