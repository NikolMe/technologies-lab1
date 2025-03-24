namespace Technologies.Models;

public class BotanicGardenPlant : BaseEntity
{
    public int GardenId { get; set; }

    public virtual BotanicGarden Garden { get; set; }

    public int PlantId { get; set; }

    public virtual Plant Plant { get; set; }
}