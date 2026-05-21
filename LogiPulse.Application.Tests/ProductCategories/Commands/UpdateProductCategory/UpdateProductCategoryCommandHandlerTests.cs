using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Application.ProductCategories.Commands.UpdateProductCategory;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using NSubstitute;

namespace LogiPulse.Application.Tests.ProductCategories.Commands.UpdateProductCategory;

public class UpdateProductCategoryCommandHandlerTests
{
    private readonly IProductCategoryRepository _productCategoryRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateProductCategoryCommandHandler _handler;
    private readonly UpdateProductCategoryCommand _command;
    private readonly Guid _tenantId;

    public UpdateProductCategoryCommandHandlerTests()
    {
        _productCategoryRepositoryMock = Substitute.For<IProductCategoryRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        _tenantId = Guid.CreateVersion7();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new UpdateProductCategoryCommandHandler(_productCategoryRepositoryMock, userContextMock,
            _unitOfWorkMock);

        _command = new UpdateProductCategoryCommand
        {
            Name = "Westroot Warehouse"
        };
    }

    [Fact]
    public async Task Handle_WhenValid_UpdatesProductCategoryAndCommits()
    {
        var productCategory = ProductCategory.Create(
            _tenantId,
            "ID-0001",
            "Old Name"
        );

        _command.Id = productCategory.Id;

        _productCategoryRepositoryMock
            .GetByIdAsync(productCategory.Id, Arg.Any<CancellationToken>())
            .Returns(productCategory);

        var result = await _handler.Handle(_command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(_command.Name);

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProductCategoryDontExist_ShouldThrow()
    {
        _productCategoryRepositoryMock
            .GetByIdAsync(_command.Id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<ProductCategory?>(null));

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Product category not found*");
    }
}