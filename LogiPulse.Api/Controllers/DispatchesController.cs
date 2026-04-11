using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using LogiPulse.Domain.Entities.Dispatches;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DispatchesController : ControllerBase
{
    // An action method that handles HTTP GET requests
    [HttpGet]
    public IActionResult Get()
    {
        var externalId = RandomNumberGenerator.GetHexString(16);
        return Ok(Dispatch.Create(externalId)); // Returns an HTTP 200 OK status with a message
    }
}