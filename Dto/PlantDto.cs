using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Technologies.Dto;

public class PlantDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [AllowNull]
    [StringLength(100)]
    public string LatinName { get; set; }

    [AllowNull]
    [StringLength(50)]
    public string Genus { get; set; }

    [AllowNull]
    [StringLength(100)]
    public string Family { get; set; }

    [AllowNull]
    [StringLength(100)]
    public string Origin { get; set; }

    [AllowNull]
    [StringLength(200)]
    public string Description { get; set; }

    [AllowNull]
    [StringLength(200)]
    public string Flower { get; set; }

    [AllowNull]
    [StringLength(200)]
    public string Fruit { get; set; }

    [AllowNull]
    [StringLength(200)]
    public string Leaf { get; set; }

    [AllowNull]
    [StringLength(200)]
    public string Usage { get; set; }
}