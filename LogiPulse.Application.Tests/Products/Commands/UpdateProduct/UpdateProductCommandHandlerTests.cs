using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Application.Products.Commands.UpdateProduct;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using NSubstitute;

namespace LogiPulse.Application.Tests.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandlerTests
{
    private readonly IProductRepository _productRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateProductCommandHandler _handler;
    private readonly UpdateProductCommand _command;
    private readonly Guid _tenantId = Guid.CreateVersion7();
    private readonly Guid _productCategoryId = Guid.CreateVersion7();

    public UpdateProductCommandHandlerTests()
    {
        _productRepositoryMock = Substitute.For<IProductRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new UpdateProductCommandHandler(_productRepositoryMock, userContextMock,
            _unitOfWorkMock);

        _command = new UpdateProductCommand
        {
            Name = "Westroot Warehouse"
        };
    }

    [Fact]
    public async Task Handle_WhenValid_UpdatesProductAndCommits()
    {
        var product = Product.Create(Guid.CreateVersion7(), "D-01", "3791", "Product", _productCategoryId);


        _command.Id = product.Id;

        _productRepositoryMock
            .GetByIdAsync(product.Id, Arg.Any<CancellationToken>())
            .Returns(product);

        var result = await _handler.Handle(_command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(_command.Name);

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProductDontExist_ShouldThrow()
    {
        _productRepositoryMock
            .GetByIdAsync(_command.Id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(null));

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Product not found*");
    }
}