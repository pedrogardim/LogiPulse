using LogiPulse.Application.Products.Commands.CreateProduct;
using LogiPulse.Application.Products.Commands.DeleteProduct;
using LogiPulse.Application.Products.Commands.UpdateProduct;
using LogiPulse.Application.Products.Queries.ListProducts;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] ListProductsQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateProductCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}