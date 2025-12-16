namespace LeadRouteOptimizer.Application.Interfaces
{
    public interface ILeadCsvParser
    {
        Task<CsvParseResult> ParseAsync(Stream csvStream, CancellationToken ct);
    }

    public record CsvParseResult(
        IReadOnlyList<ParsedLeadRow> Rows,
        IReadOnlyList<string> HeaderNames
    );

    public record ParsedLeadRow(
        int RowNumber,
        string? LeadName,
        decimal? Latitude,
        decimal? Longitude,
        string? Street,
        string? City,
        string? State,
        string? Zip,
        string Raw
    );
}
