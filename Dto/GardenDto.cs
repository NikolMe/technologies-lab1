using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;
using Technologies.Models;

namespace Technologies.Dto;

public class GardenDto
{
    [Required]
    [SwaggerSchema("Name of the garden")]
    public string Name { get; set; }

    [Required]
    [SwaggerSchema("Name of the located city")]
    public string City { get; set; }

    [Required]
    [SwaggerSchema("Type of the garden")]
    public GardenType Type { get; set; }

    [Range(1800, 2030, ErrorMessage = "Year must be between 1800 and 2030.")]
    public int YearFounded { get; set; }

    [Required]
    [MaxLength(100)]
    public string Address { get; set; }

    [AllowNull]
    [MaxLength(15)]
    public string ContactPhone { get; set; }
}