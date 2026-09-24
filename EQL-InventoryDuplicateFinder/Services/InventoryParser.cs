using System.Globalization;
using InventoryDuplicateFinder.Models;

namespace InventoryDuplicateFinder.Services;

public sealed class InventoryParser
{
    private const string KeyRingHeaderPrefix = "KeyRing\tName\tID";

    public ParseResult Parse(string filePath)
    {
        List<InventoryRow> rows = [];
        List<ParseWarning> warnings = [];
        int warningCount = 0;
        bool isKeyRingSection = false;

        string[] lines = File.ReadAllLines(filePath);
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith(KeyRingHeaderPrefix, StringComparison.OrdinalIgnoreCase))
            {
                isKeyRingSection = true;
                continue;
            }

            string[] parts = line.Split('\t');
            int minimumColumns = isKeyRingSection ? 3 : 5;
            if (parts.Length < minimumColumns)
            {
                warningCount++;
                warnings.Add(new ParseWarning
                {
                    LineNumber = i + 1,
                    Message = isKeyRingSection
                        ? "Expected at least 3 tab-delimited columns in KeyRing section."
                        : "Expected 5 tab-delimited columns.",
                    RawLine = line
                });
                continue;
            }

            string location = parts[0].Trim();
            string name = parts[1].Trim();
            string idRaw = parts[2].Trim();
            string countRaw = isKeyRingSection ? "0" : parts[3].Trim();
            string slotsRaw = isKeyRingSection ? "0" : parts[4].Trim();

            if (!int.TryParse(idRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int id))
            {
                warningCount++;
                warnings.Add(new ParseWarning
                {
                    LineNumber = i + 1,
                    Message = "ID is not a valid integer.",
                    RawLine = line
                });
                continue;
            }

            int count = 0;
            if (!int.TryParse(countRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out count))
            {
                warningCount++;
                warnings.Add(new ParseWarning
                {
                    LineNumber = i + 1,
                    Message = "Count is not a valid integer.",
                    RawLine = line
                });
            }

            int slots = 0;
            if (!int.TryParse(slotsRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out slots))
            {
                warningCount++;
                warnings.Add(new ParseWarning
                {
                    LineNumber = i + 1,
                    Message = "Slots is not a valid integer.",
                    RawLine = line
                });
            }

            rows.Add(new InventoryRow
            {
                LineNumber = i + 1,
                Location = location,
                Name = name,
                Id = id,
                Count = count,
                Slots = slots
            });
        }

        return new ParseResult
        {
            Rows = rows,
            WarningCount = warningCount,
            Warnings = warnings
        };
    }
}
