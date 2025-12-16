using LeadRouteOptimizer.Api.Contracts;
using LeadRouteOptimizer.Application.Plans.Commands;
using LeadRouteOptimizer.Application.Uploads.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LeadRouteOptimizer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlansController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreatePlanResult>> Create(
            [FromBody] CreatePlanRequest req,
            [FromServices] CreatePlanHandler handler,
            CancellationToken ct)
        {
            try
            {
                var result = await handler.HandleAsync(
                    new CreatePlanCommand(req.HomeLatitude, req.HomeLongitude, req.UploadBatchIds),
                    ct);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{planId:guid}")]
        public async Task<ActionResult<GetPlanResult>> Get(
            [FromRoute] Guid planId,
            [FromServices] GetPlanHandler handler,
            CancellationToken ct)
        {
            var result = await handler.HandleAsync(new GetPlanQuery(planId), ct);
            if (result is null) return NotFound();
            return Ok(result);
        }
    }
}
