﻿using Domain.Entities;

namespace Application.UseCases.UserWishlistOperations;

public class WishlistResponseModel
{
    public Guid WishlistId { get; set; }
    public int GameId { get; set; }
    private Game Game { get; set; }
    public string game => Game.Title;
}
 
