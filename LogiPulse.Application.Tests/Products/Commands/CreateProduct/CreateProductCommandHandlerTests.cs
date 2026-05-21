using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Application.Products.Commands.CreateProduct;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using NSubstitute;

namespace LogiPulse.Application.Tests.Products.Commands.CreateProduct;

public class CreateProductCommandHandlerTests
{
    private readonly IProductRepository _productRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateProductCommandHandler _handler;
    private readonly CreateProductCommand _command;
    private readonly Guid _tenantId = Guid.CreateVersion7();
    private readonly ProductRequirement _productRequirement = new("TEMP", "C", 2, 4);

    public CreateProductCommandHandlerTests()
    {
        _productRepositoryMock = Substitute.For<IProductRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new CreateProductCommandHandler(_productRepositoryMock, userContextMock,
            _unitOfWorkMock);

        _command = new CreateProductCommand
        {
            ExternalId = "F-00001",
            Name = "Westroot Warehouse",
            Code = "2039",
            CategoryId = Guid.CreateVersion7(),
            ProductRequirements = [_productRequirement]
        };
    }

    [Fact]
    public async Task Handle_ShouldCreateProduct()
    {
        Product? product = null;

        _productRepositoryMock
            .When(x => x.AddAsync(Arg.Any<Product>(), CancellationToken.None))
            .Do(callInfo => product = callInfo.Arg<Product>());

        var result = await _handler.Handle(_command, CancellationToken.None);
        result.Should().NotBeEmpty();

        product!.TenantId.Should().Be(_tenantId);
        product!.ExternalId.Should().Be(_command.ExternalId);
        product!.Name.Should().Be(_command.Name);
        product!.Code.Should().Be(_command.Code);
        product!.CategoryId.Should().Be(_command.CategoryId);

        product!.Requirements.First().RuleUnit.Should().Be(_productRequirement.RuleUnit);
        product!.Requirements.First().Metric.Should().Be(_productRequirement.Metric);
        product!.Requirements.First().Min.Should().Be(_productRequirement.Min);
        product!.Requirements.First().Max.Should().Be(_productRequirement.Max);

        product.Should().NotBeNull();

        await _productRepositoryMock.Received(1)
            .AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProductAlreadyExistsWithSameExternalId_ShouldThrow()
    {
        _productRepositoryMock
            .ExistsByExternalIdAsync(_command.ExternalId, CancellationToken.None)
            .Returns(true);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*A product with that external id already exists*");
    }

    [Fact]
    public async Task Handle_WhenProductAlreadyExistsWithSameCode_ShouldThrow()
    {
        _productRepositoryMock
            .ExistsByCodeAsync(_command.Code, CancellationToken.None)
            .Returns(true);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*A product with that code already exists*");
    }
}