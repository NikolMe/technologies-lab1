using System.ComponentModel.DataAnnotations;

namespace Technologies.Dto;

public class PlantsDto
{
    [Required, MinLength(0), MaxLength(10)]
    public int[] PlantIds { get; set; }
}