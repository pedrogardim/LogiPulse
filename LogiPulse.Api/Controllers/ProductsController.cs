using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using LogiPulse.Domain.Entities.Dispatch;
using LogiPulse.Domain.Entities.Product;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // An action method that handles HTTP GET requests
    [HttpGet]
    public IActionResult Get()
    {
        var extCategoryId = RandomNumberGenerator.GetHexString(16);
        var category = ProductCategory.Create(extCategoryId, "Category");

        var externalId = RandomNumberGenerator.GetHexString(16);
        var code = RandomNumberGenerator.GetHexString(16);
        
        return Ok(Product.Create(externalId, code, "Demo Product", category.Id)); // Returns an HTTP 200 OK status with a message
    }
}