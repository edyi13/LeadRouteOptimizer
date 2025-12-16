using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Application.Interfaces
{
    public interface ILeadCsvParser
    {
        Task<CsvParseResult> ParseAsync(Stream csvStream, CancellationToken ct);
    }

    public sealed record CsvParseResult(
        IReadOnlyList<ParsedLeadRow> Rows,
        IReadOnlyList<string> HeaderNames
    );

    public sealed record ParsedLeadRow(
        int RowNumber,
        string? LeadName,
        decimal? Latitude,
        decimal? Longitude,
        string? Street,
        string? City,
        string? State,
        string? Zip,
        string Raw // raw row representation for debug/audit
    );
}
