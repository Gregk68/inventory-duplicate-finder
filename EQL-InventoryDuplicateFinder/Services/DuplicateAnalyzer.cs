using InventoryDuplicateFinder.Models;

namespace InventoryDuplicateFinder.Services;

public sealed class DuplicateAnalyzer
{
    public List<DuplicateGroup> FindDuplicateIds(IEnumerable<InventoryRow> rows, HashSet<int> ignoredIds, bool excludeZeroIds)
    {
        IEnumerable<InventoryRow> filtered = rows;

        if (excludeZeroIds)
        {
            filtered = filtered.Where(r => r.Id != 0);
        }

        if (ignoredIds.Count > 0)
        {
            filtered = filtered.Where(r => !ignoredIds.Contains(r.Id));
        }

        return filtered
            .GroupBy(r => r.Id)
            .Where(g => g.Count() > 1)
            .Select(g => new DuplicateGroup
            {
                Id = g.Key,
                Occurrences = g.Count(),
                Names = g.Select(r => r.Name).Where(n => !string.IsNullOrWhiteSpace(n)).Distinct().OrderBy(n => n).ToList(),
                Locations = g.Select(r => r.Location).Where(l => !string.IsNullOrWhiteSpace(l)).Distinct().OrderBy(l => l).ToList()
            })
            .OrderByDescending(g => g.Occurrences)
            .ThenBy(g => g.Id)
            .ToList();
    }
}
