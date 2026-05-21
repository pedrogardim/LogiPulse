using LogiPulse.Domain.Entities.Products;
using MediatR;

namespace LogiPulse.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<Guid>
{
    public string ExternalId;
    public string Name;
    public string Code;
    public Guid CategoryId;
    public IReadOnlyCollection<ProductRequirement>? ProductRequirements;
}