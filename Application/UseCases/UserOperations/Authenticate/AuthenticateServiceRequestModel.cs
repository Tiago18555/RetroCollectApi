﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.UserOperations.Authenticate;

public class AuthenticateServiceRequestModel
{
    [JsonPropertyName("username")]
    [MinLength(3)]
    [MaxLength(20)]
    [Required(ErrorMessage = "O campo usuário não pode estar vazio")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    [MinLength(6)]
    [Required(ErrorMessage = "O campo senha não pode estar vazio.")]
    public string Password { get; set; }

}
