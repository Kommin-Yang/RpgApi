namespace RpgApi.Models;

public class ItemStat
{
    public int Id { get; set; }

    public int ItemInstanceId { get; set; }

    public StatType StatType { get; set; }

    public double Value { get; set; }
}