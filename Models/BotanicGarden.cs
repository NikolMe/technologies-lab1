namespace Technologies.Models;

public class BotanicGarden : BaseEntity
{
    public string Name { get; set; }

    public string City { get; set; }

    public GardenType Type { get; set; }

    public int YearFounded { get; set; }

    public string Address { get; set; }

    public string ContactPhone { get; set; }

    public virtual ICollection<BotanicGardenPlant> GardenPlants { get; set; }
}