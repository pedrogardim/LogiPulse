using FluentAssertions;
using LogiPulse.Application.Products.Commands.CreateProduct;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.Products.Commands.CreateProduct;

public class CreateProductCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private readonly ProductRequirement _productRequirement = new("TEMP", "C", 2, 4);

    [Fact]
    public async Task Handle_ShouldCreateProduct()
    {
        var command = new CreateProductCommand
        {
            ExternalId = Guid.CreateVersion7().ToString()[0..20],
            Name = "Some Product",
            Code = Guid.CreateVersion7().ToString()[0..10],
            ProductRequirements = [_productRequirement]
        };

        var productId = await Sender.Send(command, CancellationToken.None);
        productId.Should().NotBeEmpty();

        var product = await DbContext.Products.FirstOrDefaultAsync(d => d.Id == productId);
        product.Should().NotBeNull();

        product!.TenantId.Should().Be(UserContext.TenantId);
        product!.ExternalId.Should().Be(command.ExternalId);
        product!.Name.Should().Be(command.Name);
        product!.Code.Should().Be(command.Code);
        product!.CategoryId.Should().Be(command.CategoryId);

        product!.Requirements.First().RuleUnit.Should().Be(_productRequirement.RuleUnit);
        product!.Requirements.First().Metric.Should().Be(_productRequirement.Metric);
        product!.Requirements.First().Min.Should().Be(_productRequirement.Min);
        product!.Requirements.First().Max.Should().Be(_productRequirement.Max);
    }

    [Fact]
    public async Task Handle_WhenProductAlreadyExistsWithSameExternalId_ShouldThrow()
    {
        var command = new CreateProductCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..20],
            Name = "Some Product",
            Code = Guid.NewGuid().ToString()[0..10]
        };

        await Sender.Send(command);

        var command2 = new CreateProductCommand
        {
            ExternalId = command.ExternalId,
            Name = "Some Product",
            Code = Guid.NewGuid().ToString()[0..10]
        };

        Func<Task> act = async () => await Sender.Send(command2);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*A product with that external id already exists*");
    }

    [Fact]
    public async Task Handle_WhenProductAlreadyExistsWithSameCode_ShouldThrow()
    {
        var command = new CreateProductCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..20],
            Name = "Some Product",
            Code = Guid.NewGuid().ToString()[0..10]
        };

        await Sender.Send(command);

        var command2 = new CreateProductCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..20],
            Name = "Some Product",
            Code = command.Code
        };

        Func<Task> act = async () => await Sender.Send(command2);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*A product with that code already exists*");
    }
}