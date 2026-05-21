using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IUserContext userContext,
    IUnitOfWork uow
) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            throw new NotFoundException("Product don't exist");

        productRepository.Remove(product);

        await uow.CommitAsync(cancellationToken);
    }
}