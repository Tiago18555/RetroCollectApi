﻿using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class Rating
    {
        [Key]
        public Guid RatingId { get; set; }
        public Guid UserId { get; set; }
        public int GameId { get; set; }
        public int RatingValue { get; set; }
        public string Review { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
