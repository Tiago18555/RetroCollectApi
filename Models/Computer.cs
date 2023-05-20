﻿using RetroCollect.CrossCutting.Enums;
using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class Computer
    {
        [Key]
        public Guid ComputerId { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Name { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public Manufacturer Manufacturer { get; set; }
        public int ReleaseYear { get; set; }

        [MinLength(3)]
        [MaxLength(2048)]
        public string Description { get; set; }

        [MinLength(1)]
        public string ImageUrl { get; set; }

        public ICollection<Game> Games { get; set; }
        public ICollection<UserComputer> UserComputers { get; set; }
    }
}
