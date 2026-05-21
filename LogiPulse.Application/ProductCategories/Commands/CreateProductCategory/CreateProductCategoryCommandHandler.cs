using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.ProductCategories.Commands.CreateProductCategory;

public class CreateProductCategoryCommandHandler(
    IProductCategoryRepository productCategoryRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateProductCategoryCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var existsByTenantId =
            await productCategoryRepository.ExistsByExternalIdAsync(request.ExternalId, cancellationToken);

        if (existsByTenantId)
            throw new ConflictException("A product with that external id already exists");

        var productCategory = ProductCategory.Create(
            tenantId,
            request.ExternalId,
            request.Name
        );

        if (request.ProductRequirements != null)
            foreach (var requirement in request.ProductRequirements)
                productCategory.SetRequirement(
                    requirement.Metric.Code,
                    requirement.RuleUnit.Symbol,
                    requirement.Min,
                    requirement.Max
                );

        await productCategoryRepository.AddAsync(productCategory, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return productCategory.Id;
    }
}