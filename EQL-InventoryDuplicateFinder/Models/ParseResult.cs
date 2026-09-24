namespace InventoryDuplicateFinder.Models;

public sealed class ParseResult
{
    public List<InventoryRow> Rows { get; init; } = [];
    public int WarningCount { get; init; }
    public List<ParseWarning> Warnings { get; init; } = [];
}
