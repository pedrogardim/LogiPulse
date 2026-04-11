using Microsoft.AspNetCore.Mvc;
using LogiPulse.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DispatchesController : ControllerBase
{
    private readonly LogiPulseDbContext _context;

    public DispatchesController(LogiPulseDbContext dbContext)
    {
        _context = dbContext;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var dispatches = _context
            .Dispatches
            .Include(d => d.Product)
            .Include(d => d.Product.Category)
            .ToList();

        return Ok(dispatches);
    }
}