using Microsoft.AspNetCore.Mvc;
using LogiPulse.Domain.Entities;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    // An action method that handles HTTP GET requests
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new Order{}); // Returns an HTTP 200 OK status with a message
    }
}