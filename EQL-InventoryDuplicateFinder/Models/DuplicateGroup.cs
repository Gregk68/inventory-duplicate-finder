namespace InventoryDuplicateFinder.Models;

public sealed class DuplicateGroup
{
    public int Id { get; init; }
    public int Occurrences { get; init; }
    public IReadOnlyList<string> Names { get; init; } = [];
    public IReadOnlyList<string> Locations { get; init; } = [];
}
