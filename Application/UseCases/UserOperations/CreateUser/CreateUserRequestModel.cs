﻿using System.ComponentModel.DataAnnotations;
using CrossCutting.Validations;

namespace Application.UseCases.UserOperations.CreateUser;

public class CreateUserRequestModel
{
    [IsValidUri]
    [MinLength(3)]
    [MaxLength(255)]
    [Required(ErrorMessage = "O campo \"username\" é obrigatório")]
    public string Username { get; set; }

    [Required(ErrorMessage = "\"O campo \"password\" é obrigatório\"")]
    [MinLength(6)]
    [MaxLength(255)]
    public string Password { get; set; }

    [Required(ErrorMessage = "\"O campo \"password\" é obrigatório\"")]
    [MinLength(6)]
    [MaxLength(255)]
    [Compare("Password", ErrorMessage = "Os campos \"password\" e \"confirm password\" devem ser iguais")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "")]
    [EmailAddress(ErrorMessage = "O campo \"email\" está com o formato inválido")]
    public string Email { get; set; }

    [MinLength(3)]
    [MaxLength(255)]
    [Required(ErrorMessage = "\"O campo \"first name\" é obrigatório\"")]
    public string FirstName { get; set; }

    [MinLength(3)]
    [MaxLength(255)]
    public string LastName { get; set; }
}
