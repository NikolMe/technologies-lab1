using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Technologies.Database;
using Technologies.Dto;
using Technologies.Models;

namespace Technologies.Controllers;

[ApiController]
[Route("[controller]")]
[SwaggerTag("Operations related to plants")]
public class PlantsController : ControllerBase
{
    private readonly IRepository<Plant> _repository;

    public PlantsController(IRepository<Plant> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Plant[]))]
    public async Task<ActionResult<Plant[]>> GetAllPlants()
    {
        var result = await _repository.GetArrayAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Plant))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Plant>> GetPlantById([FromRoute] long id)
    {
        var result = await _repository.FirstOrDefaultAsync(x => x.Id == id);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    public async Task<ActionResult<int>> CreatePlant([FromBody] PlantDto plant)
    {
        var newPlant = new Plant
        {
            Name = plant.Name,
            Description = plant.Description,
            Family = plant.Family,
            Fruit = plant.Fruit,
            Genus = plant.Genus,
            Flower = plant.Flower,
            LatinName = plant.LatinName,
            Origin = plant.Origin,
            Leaf = plant.Leaf,
            Usage = plant.Usage,
        };
        var id = await _repository.AddEntityAsync(newPlant);

        return Ok(id);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdatePlant([FromRoute] long id, [FromBody] PlantDto plant)
    {
        await _repository.ExecuteUpdateAsync(
            x => x.Id == id,
            x => x
                .SetProperty(p => p.Name, plant.Name)
                .SetProperty(p => p.Family, plant.Family)
                .SetProperty(p => p.Description, plant.Description)
                .SetProperty(p => p.Origin, plant.Origin)
                .SetProperty(p => p.LatinName, plant.LatinName)
                .SetProperty(p => p.Leaf, plant.Leaf)
                .SetProperty(p => p.Flower, plant.Flower)
                .SetProperty(p => p.Fruit, plant.Fruit)
                .SetProperty(p => p.Usage, plant.Usage));

        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeletePlant([FromRoute] long id)
    {
        await _repository.RemoveAsync(x => x.Id == id);

        return NoContent();
    }
}