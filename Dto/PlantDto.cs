using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Technologies.Dto;

public class PlantDto
{
    [Required]
    public string Name { get; set; }

    [AllowNull]
    public string LatinName { get; set; }

    [AllowNull]
    public string Genus { get; set; }

    [AllowNull]
    public string Family { get; set; }

    [AllowNull]
    public string Origin { get; set; }

    [AllowNull]
    public string Description { get; set; }

    [AllowNull]
    public string Flower { get; set; }

    [AllowNull]
    public string Fruit { get; set; }

    [AllowNull]
    public string Leaf { get; set; }

    [AllowNull]
    public string Usage { get; set; }
}