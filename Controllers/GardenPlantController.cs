using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Technologies.Database;
using Technologies.Dto;
using Technologies.Models;

namespace Technologies.Controllers;

[ApiController]
[Route("plants/gardens")]
[SwaggerTag("Operations related to plants of a garden")]
public class GardenPlantController : ControllerBase
{
    private readonly IRepository<BotanicGardenPlant> _repository;
    private readonly IRepository<BotanicGarden> _gardenRepository;
    private readonly IRepository<Plant> _plantRepository;

    public GardenPlantController(
        IRepository<BotanicGardenPlant> repository,
        IRepository<BotanicGarden> gardenRepository,
        IRepository<Plant> plantRepository)
    {
        _repository = repository;
        _gardenRepository = gardenRepository;
        _plantRepository = plantRepository;
    }

    [HttpPost("{gardenId}/addPlants")]
    [SwaggerOperation(Summary = "Add plants to the garden", Description = "Add plants to the garden by the given plant IDS")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Garden not found")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<ActionResult> AddPlants([FromRoute] int gardenId, [FromBody, MinLength(1), MaxLength(10)] PlantsDto plantsDto)
    {
        if (!await _gardenRepository.AnyAsync(x => x.Id == gardenId))
        {
            return NotFound("Cannot find garden by given ID");
        }

        var plantIds = (await _plantRepository
            .GetArrayAsync(x => plantsDto.PlantIds.Contains(x.Id)))
            .Select(x => x.Id)
            .ToList();

        var notFoundPlants = plantsDto.PlantIds.Except(plantIds).ToList();
        if (notFoundPlants.Any())
        {
            return BadRequest($"Plants {string.Join(',', notFoundPlants)} not found");
        }

        var gardenPlants = plantIds
            .Select(plantId => new BotanicGardenPlant { PlantId = plantId, GardenId = gardenId })
            .ToList();

        await _repository.AddRangeAsync(gardenPlants);

        return Ok();
    }

    [HttpDelete("{gardenId}")]
    [SwaggerOperation(Summary = "Remove plants from the garden", Description = "Remove plants from the garden by the given plant IDS")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Garden not found")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemovePlants([FromRoute] int gardenId, [FromBody, MinLength(1), MaxLength(10)] PlantsDto plantsDto)
    {
        if (!await _gardenRepository.AnyAsync(x => x.Id == gardenId))
        {
            return NotFound("Cannot find garden by given ID");
        }

        await _repository.RemoveAsync(x => x.GardenId == gardenId && plantsDto.PlantIds.Contains(x.PlantId));

        return NoContent();
    }

    [HttpGet("{gardenId}")]
    [SwaggerOperation(Summary = "Get all plants for the garden")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Garden not found")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GardenPlantDto[]))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GardenPlantDto[]>> GetPlants([FromRoute] int gardenId)
    {
        if (!await _gardenRepository.AnyAsync(x => x.Id == gardenId))
        {
            return NotFound("Cannot find garden by given ID");
        }

        var results = await _repository.GetArrayAsync(
            x => x.GardenId == gardenId,
            x => x.Include(p => p.Plant));

        return Ok(results.Select(MapToDto));
    }

    private static GardenPlantDto MapToDto(BotanicGardenPlant gardenPlant)
    {
        return new GardenPlantDto
        {
            PlantId = gardenPlant.PlantId,
            PlantName = gardenPlant.Plant.Name
        };
    }
}