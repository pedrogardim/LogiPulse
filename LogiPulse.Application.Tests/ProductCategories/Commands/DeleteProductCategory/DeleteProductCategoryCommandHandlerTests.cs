using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Application.ProductCategories.Commands.DeleteProductCategory;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using NSubstitute;

namespace LogiPulse.Application.Tests.ProductCategories.Commands.DeleteProductCategory;

public class DeleteProductCategoryCommandHandlerTests
{
    private readonly IProductCategoryRepository _facilityRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteProductCategoryCommandHandler _handler;
    private readonly Guid _tenantId;

    public DeleteProductCategoryCommandHandlerTests()
    {
        _facilityRepositoryMock = Substitute.For<IProductCategoryRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        _tenantId = Guid.CreateVersion7();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new DeleteProductCategoryCommandHandler(
            _facilityRepositoryMock,
            userContextMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteProductCategoryAndCommits()
    {
        var facility = ProductCategory.Create(_tenantId, "D-01", "Category");

        var command = new DeleteProductCategoryCommand(facility.Id);

        _facilityRepositoryMock
            .GetByIdAsync(facility.Id, Arg.Any<CancellationToken>())
            .Returns(facility);

        await _handler.Handle(command, CancellationToken.None);

        _facilityRepositoryMock.Received(1).Remove(facility);

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProductCategoryDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        _facilityRepositoryMock
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<ProductCategory?>(null));

        var act = async () => await _handler.Handle(new DeleteProductCategoryCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Product category don't exist*");
    }
}