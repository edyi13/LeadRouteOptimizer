using Microsoft.AspNetCore.Mvc;

namespace LeadRouteOptimizer.Api.Contracts
{
    public class UploadLeadsRequest
    {
        [FromForm(Name = "sourceType")]
        public string SourceType { get; set; } = default!;

        [FromForm(Name = "file")]
        public IFormFile File { get; set; } = default!;
    }
}
