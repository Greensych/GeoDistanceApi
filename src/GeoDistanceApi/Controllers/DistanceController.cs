using GeoDistanceApi.Models;
using GeoDistanceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoDistanceApi.Controllers;

[ApiController]
[Route("api/distance")]
public class DistanceController : ControllerBase
{
    private readonly IDistanceService _distanceService;

    public DistanceController(IDistanceService distanceService)
    {
        _distanceService = distanceService;
    }

    [HttpPost("calculate")]
    public IActionResult CalculateDistance([FromBody] DistanceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _distanceService.GetDistanceKm(request.From, request.To);
        return Ok(new { distanceKm = result });
    }
}
