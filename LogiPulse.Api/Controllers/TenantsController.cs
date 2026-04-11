using Microsoft.AspNetCore.Mvc;
using LogiPulse.Infrastructure.Persistance;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly LogiPulseDbContext _context;
    
    public TenantsController(LogiPulseDbContext dbContext)
    {
        _context = dbContext;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        var dispatches = _context.Tenants.ToList();
        return Ok(dispatches);
    }
}