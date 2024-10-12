using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

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

    [MinLength(1)]
    public string ImageUrl { get; set; }

    public bool IsPortable { get; set; }
    public IEnumerable<UserConsole> UserConsoles { get; set; }
}
