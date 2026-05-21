using LogiPulse.Application.ProductCategories.Commands.CreateProductCategory;
using LogiPulse.Application.ProductCategories.Commands.DeleteProductCategory;
using LogiPulse.Application.ProductCategories.Commands.UpdateProductCategory;
using LogiPulse.Application.ProductCategories.Queries.ListProductCategories;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductCategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] ListProductCategoriesQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateProductCategoryCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await mediator.Send(new DeleteProductCategoryCommand(id));
        return NoContent();
    }
}