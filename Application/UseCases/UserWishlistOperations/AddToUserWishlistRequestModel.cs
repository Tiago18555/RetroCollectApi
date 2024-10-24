using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.UserWishlistOperations;

public class AddToUserWishlistRequestModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
