using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.ProductCategories.Commands.UpdateProductCategory;

public class UpdateProductCategoryCommandHandler(
    IProductCategoryRepository productCategoryRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateProductCategoryCommand, ProductCategory>
{
    public async Task<ProductCategory> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var productCategory = await productCategoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (productCategory is null)
            throw new NotFoundException("Product not found");

        productCategory.Update(request.Name);

        await unitOfWork.CommitAsync(cancellationToken);

        return productCategory;
    }
}