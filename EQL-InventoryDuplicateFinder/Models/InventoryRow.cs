namespace InventoryDuplicateFinder.Models;

public sealed class InventoryRow
{
    public int LineNumber { get; init; }
    public string Location { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int Id { get; init; }
    public int Count { get; init; }
    public int Slots { get; init; }
}
