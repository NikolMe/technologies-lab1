using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Technologies.Database;
using Technologies.Dto;
using Technologies.Models;

namespace Technologies.Controllers;

[ApiController]
[Route("[controller]")]
[SwaggerTag("Operations related to gardens")]
public class GardensController : ControllerBase
{
    private readonly IRepository<BotanicGarden> _repository;

    public GardensController(IRepository<BotanicGarden> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all botanic gardens")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BotanicGarden[]))]
    public async Task<ActionResult<BotanicGarden[]>> GetAllBotanicGardens()
    {
        var result = await _repository.GetArrayAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a botanic garden by ID")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BotanicGarden))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<ActionResult<BotanicGarden>> GetBotanicGardenById([FromRoute] long id)
    {
        var result = await _repository.FirstOrDefaultAsync(x => x.Id == id);

        return result is null ? NotFound("Botanic garden is not found") : Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a botanic garden")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    public async Task<ActionResult<int>> CreateBotanicGarden([FromBody] GardenDto garden)
    {
        var newGarden = new BotanicGarden
        {
            Name = garden.Name,
            YearFounded = garden.YearFounded,
            City = garden.City,
            Type = garden.Type,
            Address = garden.Address,
            ContactPhone = garden.ContactPhone,
        };
        var id = await _repository.AddEntityAsync(newGarden);

        return Ok(id);
    }

    [HttpPut("{id}")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Garden not found")]
    [SwaggerOperation(Summary = "Update info of a botanic garden")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<ActionResult> UpdateBotanicGarden([FromRoute] long id, [FromBody] GardenDto garden)
    {
        if (!await _repository.AnyAsync(x => x.Id == id))
        {
            return NotFound("Cannot find botanic garden by given ID");
        }

        await _repository.ExecuteUpdateAsync(
            x => x.Id == id,
            x => x
                .SetProperty(p => p.Name, garden.Name)
                .SetProperty(p => p.YearFounded, garden.YearFounded)
                .SetProperty(p => p.City, garden.City)
                .SetProperty(p => p.Type, garden.Type)
                .SetProperty(p => p.Address, garden.Address)
                .SetProperty(p => p.ContactPhone, garden.ContactPhone)
            );

        return Ok();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a botanic garden")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteBotanicGarden([FromRoute] long id)
    {
        await _repository.RemoveAsync(x => x.Id == id);

        return NoContent();
    }
}