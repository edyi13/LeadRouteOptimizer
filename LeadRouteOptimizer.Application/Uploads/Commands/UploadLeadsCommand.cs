using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadRouteOptimizer.Application.Uploads.Commands
{
    public sealed record UploadLeadsCommand(
        string SourceType,
        string? OriginalFileName,
        Stream CsvStream
    );
}
