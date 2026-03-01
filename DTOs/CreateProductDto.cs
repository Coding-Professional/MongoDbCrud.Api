using System.ComponentModel.DataAnnotations;

namespace MongoDbCrud.Api.DTOs;

public class CreateProductDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 999999)]
    public decimal Price { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;

    public bool InStock { get; set; } = true;
}
