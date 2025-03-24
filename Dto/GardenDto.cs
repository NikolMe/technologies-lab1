using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Technologies.Models;

namespace Technologies.Dto;

public class GardenDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public GardenType Type { get; set; }

    public int YearFounded { get; set; }

    [Required]
    public string Address { get; set; }

    public string ContactPhone { get; set; }
}