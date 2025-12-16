using CsvHelper;
using CsvHelper.Configuration;
using LeadRouteOptimizer.Application.Interfaces;
using System.Globalization;

namespace LeadRouteOptimizer.Infrastructure.Adapters
{
    internal class CsvLeadParser : ILeadCsvParser
    {
        public async Task<CsvParseResult> ParseAsync(Stream csvStream, CancellationToken ct)
        {
            using var reader = new StreamReader(csvStream, leaveOpen: true);

            var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null,
                MissingFieldFound = null,
                HeaderValidated = null
            };

            using var csv = new CsvReader(reader, cfg);

            await csv.ReadAsync();
            csv.ReadHeader();

            var headers = csv.HeaderRecord?.ToList() ?? new List<string>();

            // required headers that are case-insensitive
            RequireHeader(headers, "LeadName");
            RequireHeader(headers, "Latitude");
            RequireHeader(headers, "Longitude");

            var rows = new List<ParsedLeadRow>();
            var rowNumber = 1;

            while (await csv.ReadAsync())
            {
                ct.ThrowIfCancellationRequested();

                string raw = string.Join(",", headers.Select(h => csv.GetField(h)));

                rows.Add(new ParsedLeadRow(
                    RowNumber: rowNumber++,
                    LeadName: GetString(csv, "LeadName"),
                    Latitude: GetDecimal(csv, "Latitude"),
                    Longitude: GetDecimal(csv, "Longitude"),
                    Street: GetString(csv, "Street"),
                    City: GetString(csv, "City"),
                    State: GetString(csv, "State"),
                    Zip: GetString(csv, "Zip"),
                    Raw: raw
                ));
            }

            return new CsvParseResult(rows, headers);
        }

        private static void RequireHeader(IReadOnlyList<string> headers, string name)
        {
            if (!headers.Any(h => string.Equals(h, name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Missing required CSV header: {name}");
        }

        private static string? GetString(CsvReader csv, string name)
        {
            if (!csv.TryGetField(name, out string? v)) return null;
            return string.IsNullOrWhiteSpace(v) ? null : v;
        }

        private static decimal? GetDecimal(CsvReader csv, string name)
        {
            if (!csv.TryGetField(name, out string? v)) return null;
            if (string.IsNullOrWhiteSpace(v)) return null;
            return decimal.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out var d) ? d : null;
        }
    }
}
