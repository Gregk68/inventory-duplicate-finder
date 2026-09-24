namespace InventoryDuplicateFinder.Models;

public sealed class ParseWarning
{
    public int LineNumber { get; init; }
    public string Message { get; init; } = string.Empty;
    public string RawLine { get; init; } = string.Empty;
}