using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Computer
{
    [Key]
    public int ComputerId { get; set; }

    [MinLength(3)]
    [MaxLength(255)]
    public string Name { get; set; }

    [MinLength(3)]
    [MaxLength(2048)]
    public string Description { get; set; }

    [MinLength(1)]
    public string ImageUrl { get; set; }

    public bool IsArcade { get; set; }
    public IEnumerable<UserComputer> UserComputers { get; set; }
}
