using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.ProductCategories.Commands.DeleteProductCategory;

public class DeleteProductCategoryCommandHandler(
    IProductCategoryRepository productCategoryRepository,
    IUserContext userContext,
    IUnitOfWork uow
) : IRequestHandler<DeleteProductCategoryCommand>
{
    public async Task Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var productCategory = await productCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (productCategory is null)
            throw new NotFoundException("Product category don't exist");

        productCategoryRepository.Remove(productCategory);

        await uow.CommitAsync(cancellationToken);
    }
}