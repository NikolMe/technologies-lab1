namespace Technologies.Models;

public class Plant : BaseEntity
{
    public string Name { get; set; }

    public string LatinName { get; set; }

    public string Genus { get; set; }

    public string Family { get; set; }

    public string Origin { get; set; }

    public string Description { get; set; }

    public string Flower { get; set; }

    public string Fruit { get; set; }

    public string Leaf { get; set; }

    public string Usage { get; set; }

    public virtual ICollection<BotanicGardenPlant> GardenPlants { get; set; }
}