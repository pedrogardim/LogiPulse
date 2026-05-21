using System.Text.Json.Serialization;
using LogiPulse.Domain.Entities.Products;
using MediatR;

namespace LogiPulse.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest<Product>
{
    [JsonIgnore] public Guid Id;
    public string? Name;
    public string? Code;
    public Guid? ProductCategoryId;
}