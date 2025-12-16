namespace LeadRouteOptimizer.Application.Uploads.Commands
{
    public record UploadLeadsCommand(
        string SourceType,
        string? OriginalFileName,
        Stream CsvStream
    );
}
