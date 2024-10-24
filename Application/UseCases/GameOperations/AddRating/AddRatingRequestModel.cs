﻿using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.GameOperations.AddRating;

public class AddRatingRequestModel
{
    [Required(ErrorMessage = "Field \"game_id\" is required")]
    public int GameId { get; set; }

    [Range(0, 100)]
    public int RatingValue { get; set; }

    [MinLength(5)]
    public string Review { get; set; }
}
