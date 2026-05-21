using LogiPulse.Domain.Entities.Products;
using MediatR;

namespace LogiPulse.Application.ProductCategories.Commands.CreateProductCategory;

public record CreateProductCategoryCommand : IRequest<Guid>
{
    public string ExternalId;
    public string Name;
    public IReadOnlyCollection<ProductRequirement>? ProductRequirements;
}