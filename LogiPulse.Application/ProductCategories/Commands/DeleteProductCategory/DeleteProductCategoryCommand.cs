using MediatR;

namespace LogiPulse.Application.ProductCategories.Commands.DeleteProductCategory;

public record DeleteProductCategoryCommand(Guid Id) : IRequest;