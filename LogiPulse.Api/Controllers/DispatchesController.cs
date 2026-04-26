using Microsoft.AspNetCore.Mvc;
using LogiPulse.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DispatchesController : ControllerBase
{
    private readonly LogiPulseDbContext _context;
    // private readonly ICurrentUserService _userService;

    public DispatchesController(
        LogiPulseDbContext dbContext 
        // ICurrentUserService userService
        )
    {
        _context = dbContext;
        // _userService = userService;
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