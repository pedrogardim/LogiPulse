using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Application.Products.Commands.DeleteProduct;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using NSubstitute;

namespace LogiPulse.Application.Tests.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandlerTests
{
    private readonly IProductRepository _productRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteProductCommandHandler _handler;
    private readonly Guid _tenantId = Guid.CreateVersion7();
    private readonly Guid _productCategoryId = Guid.CreateVersion7();

    public DeleteProductCommandHandlerTests()
    {
        _productRepositoryMock = Substitute.For<IProductRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();

        userContextMock.TenantId.Returns(_tenantId);

        _handler = new DeleteProductCommandHandler(
            _productRepositoryMock,
            userContextMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteProductAndCommits()
    {
        var product = Product.Create(Guid.CreateVersion7(), "D-01", "3791", "Product", _productCategoryId);

        var command = new DeleteProductCommand(product.Id);

        _productRepositoryMock
            .GetByIdAsync(product.Id, Arg.Any<CancellationToken>())
            .Returns(product);

        await _handler.Handle(command, CancellationToken.None);

        _productRepositoryMock.Received(1).Remove(product);

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProductDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        _productRepositoryMock
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(null));

        var act = async () => await _handler.Handle(new DeleteProductCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Product don't exist*");
    }
}