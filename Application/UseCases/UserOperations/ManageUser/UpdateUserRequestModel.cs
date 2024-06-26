﻿using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.UserOperations.ManageUser
{
    public class UpdateUserRequestModel
    {
        [Required]
        public Guid UserId { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Username { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Email { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string LastName { get; set; }

    }
}
