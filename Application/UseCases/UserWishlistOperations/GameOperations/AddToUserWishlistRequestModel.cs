using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.UserWishlistOperations.GameOperations
{
    public class AddToUserWishlistRequestModel
    {
        public int GameId { get; set; }
    }
}
