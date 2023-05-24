﻿using RetroCollectApi.CrossCutting.Enums.ForModels;
using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class Console
    {
        [Key]
        public int ConsoleId { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Name { get; set; }

        [MinLength(3)]
        [MaxLength(2048)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsPortable { get; set; }

        public ICollection<Game> Games { get; set; }
        public ICollection<UserConsole> UserConsoles { get; set; }
    }
}
