namespace RpgApi.Models;

public class ItemStats
{
    public int Id { get; set; }

    public int ItemInstanceId { get; set; }

    public StatType StatType { get; set; }

    public int Value { get; set; }
}