using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Application.ProductCategories.Commands.CreateProductCategory;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using NSubstitute;

namespace LogiPulse.IntegrationTests.ProductCategories.Commands.CreateProductCategory;

public class CreateProductCategoryCommandHandlerTests
{
    private readonly IProductCategoryRepository _productCategoryRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateProductCategoryCommandHandler _handler;
    private readonly CreateProductCategoryCommand _command;
    private readonly Guid _tenantId = Guid.CreateVersion7();
    private readonly ProductRequirement _productRequirement = new("TEMP", "C", 2, 4);

    public CreateProductCategoryCommandHandlerTests()
    {
        _productCategoryRepositoryMock = Substitute.For<IProductCategoryRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new CreateProductCategoryCommandHandler(_productCategoryRepositoryMock, userContextMock,
            _unitOfWorkMock);

        _command = new CreateProductCategoryCommand
        {
            ExternalId = "F-00001",
            Name = "Westroot Warehouse",
            ProductRequirements = [_productRequirement]
        };
    }

    [Fact]
    public async Task Handle_ShouldCreateProductCategory()
    {
        ProductCategory? productCategory = null;

        _productCategoryRepositoryMock
            .When(x => x.AddAsync(Arg.Any<ProductCategory>(), CancellationToken.None))
            .Do(callInfo => productCategory = callInfo.Arg<ProductCategory>());

        var result = await _handler.Handle(_command, CancellationToken.None);
        result.Should().NotBeEmpty();

        productCategory!.TenantId.Should().Be(_tenantId);
        productCategory!.ExternalId.Should().Be(_command.ExternalId);
        productCategory!.Name.Should().Be(_command.Name);

        productCategory!.Requirements.First().RuleUnit.Should().Be(_productRequirement.RuleUnit);
        productCategory!.Requirements.First().Metric.Should().Be(_productRequirement.Metric);
        productCategory!.Requirements.First().Min.Should().Be(_productRequirement.Min);
        productCategory!.Requirements.First().Max.Should().Be(_productRequirement.Max);

        productCategory.Should().NotBeNull();

        await _productCategoryRepositoryMock.Received(1)
            .AddAsync(Arg.Any<ProductCategory>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProductCategoryAlreadyExistsWithSameExternalId_ShouldThrow()
    {
        _productCategoryRepositoryMock
            .ExistsByExternalIdAsync(_command.ExternalId, CancellationToken.None)
            .Returns(true);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*A product category with that external id already exists");
    }
}