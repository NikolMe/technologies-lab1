using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technologies.Database;
using Technologies.Dto;
using Technologies.Models;

namespace Technologies.Controllers;

[ApiController]
[Route("[controller]")]
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
    public async Task<ActionResult> AddPlants([FromRoute] int gardenId, [FromBody] PlantsDto plantsDto)
    {
        if (!await _gardenRepository.AnyAsync(x => x.Id == gardenId))
        {
            return NotFound();
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

    [HttpPost("{gardenId}/removePlants")]
    public async Task<ActionResult> RemovePlants([FromRoute] int gardenId, [FromBody] PlantsDto plantsDto)
    {
        if (!await _gardenRepository.AnyAsync(x => x.Id == gardenId))
        {
            return NotFound();
        }

        await _repository.RemoveAsync(x => x.GardenId == gardenId && plantsDto.PlantIds.Contains(x.PlantId));

        return NoContent();
    }

    [HttpGet("{gardenId}/plants")]
    public async Task<ActionResult<GardenPlantDto[]>> GetPlants([FromRoute] int gardenId)
    {
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