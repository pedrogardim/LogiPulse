using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var existsByTenantId = await productRepository.ExistsByExternalIdAsync(request.ExternalId, cancellationToken);
        if (existsByTenantId)
            throw new ConflictException("A product with that external id already exists");

        var existsByCode = await productRepository.ExistsByCodeAsync(request.Code, cancellationToken);

        if (existsByCode)
            throw new ConflictException("A product with that code already exists");

        var product = Product.Create(
            tenantId,
            request.ExternalId,
            request.Name,
            request.Code,
            request.CategoryId
        );

        if (request.ProductRequirements != null)
            foreach (var requirement in request.ProductRequirements)
                product.SetRequirement(
                    requirement.Metric.Code,
                    requirement.RuleUnit.Symbol,
                    requirement.Min,
                    requirement.Max
                );


        await productRepository.AddAsync(product, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return product.Id;
    }
}