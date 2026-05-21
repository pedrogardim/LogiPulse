using MediatR;

namespace LogiPulse.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest;