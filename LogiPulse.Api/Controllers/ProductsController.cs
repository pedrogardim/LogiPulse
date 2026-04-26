using Microsoft.AspNetCore.Mvc;
using LogiPulse.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    
    private readonly LogiPulseDbContext _context;
    
    public ProductsController(LogiPulseDbContext dbContext)
    {
        _context = dbContext;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        var products = _context
            .Products
            .Include(p => p.Category)
            .ToList();

        return Ok(products);
    }
}