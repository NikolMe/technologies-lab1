using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technologies.Database;
using Technologies.Dto;
using Technologies.Models;

namespace Technologies.Controllers;

[ApiController]
[Route("[controller]")]
public class GardensController : ControllerBase
{
    private readonly IRepository<BotanicGarden> _repository;

    public GardensController(IRepository<BotanicGarden> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BotanicGarden>))]
    public async Task<ActionResult<BotanicGarden[]>> GetAllBotanicGardens()
    {
        var result = await _repository.GetArrayAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BotanicGarden))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BotanicGarden>> GetBotanicGardenById([FromRoute] long id)
    {
        var result = await _repository.FirstOrDefaultAsync(x => x.Id == id);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
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
    public async Task<ActionResult> UpdateBotanicGarden([FromRoute] long id, [FromBody] GardenDto garden)
    {
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
    public async Task<ActionResult> DeleteBotanicGarden([FromRoute] long id)
    {
        await _repository.RemoveAsync(x => x.Id == id);

        return Ok();
    }
}