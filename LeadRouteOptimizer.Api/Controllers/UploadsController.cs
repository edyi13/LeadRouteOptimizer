using LeadRouteOptimizer.Api.Contracts;
using LeadRouteOptimizer.Application.Uploads.Commands;
using Microsoft.AspNetCore.Mvc;

namespace LeadRouteOptimizer.Api.Endpoints
{
    [ApiController]
    [Route("[controller]")]
    public class UploadsController : ControllerBase
    {
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UploadLeadsResult>> Upload(
            [FromForm] UploadLeadsRequest req,
            [FromServices] UploadLeadsHandler handler,
            CancellationToken ct)
        {
            try
            {
                if (req.File is null || req.File.Length == 0)
                    return BadRequest("Missing file.");

                await using var stream = req.File.OpenReadStream();

                var result = await handler.HandleAsync(
                    new UploadLeadsCommand(req.SourceType, req.File.FileName, stream),
                    ct);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
