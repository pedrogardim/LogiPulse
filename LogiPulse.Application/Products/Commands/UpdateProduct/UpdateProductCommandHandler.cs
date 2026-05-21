using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateProductCommand, Product>
{
    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
            throw new NotFoundException("Product not found");

        product.Update(
            request.Name,
            request.Code,
            request.ProductCategoryId);

        await unitOfWork.CommitAsync(cancellationToken);

        return product;
    }
}