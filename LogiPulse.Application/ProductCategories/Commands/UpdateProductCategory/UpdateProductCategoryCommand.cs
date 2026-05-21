using System.Text.Json.Serialization;
using LogiPulse.Domain.Entities.Products;
using MediatR;

namespace LogiPulse.Application.ProductCategories.Commands.UpdateProductCategory;

public record UpdateProductCategoryCommand : IRequest<ProductCategory>
{
    [JsonIgnore] public Guid Id;
    public string? Name;
}