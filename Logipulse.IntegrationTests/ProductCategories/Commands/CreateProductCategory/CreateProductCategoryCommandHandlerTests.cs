using FluentAssertions;
using LogiPulse.Application.ProductCategories.Commands.CreateProductCategory;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.ProductCategories.Commands.CreateProductCategory;

public class CreateProductCategoryCommandHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private readonly ProductRequirement _productRequirement = new("TEMP", "C", 2, 4);

    [Fact]
    public async Task Handle_ShouldCreateProductCategory()
    {
        var command = new CreateProductCategoryCommand
        {
            ExternalId = Guid.CreateVersion7().ToString()[0..20],
            Name = "Some Category",
            ProductRequirements = [_productRequirement]
        };

        var productCategoryId = await Sender.Send(command);
        productCategoryId.Should().NotBeEmpty();

        var productCategory = await DbContext.ProductCategories.FirstOrDefaultAsync(d => d.Id == productCategoryId);
        productCategory.Should().NotBeNull();

        productCategory!.TenantId.Should().Be(UserContext.TenantId);
        productCategory!.ExternalId.Should().Be(command.ExternalId);
        productCategory!.Name.Should().Be(command.Name);

        productCategory!.Requirements.First().RuleUnit.Should().Be(_productRequirement.RuleUnit);
        productCategory!.Requirements.First().Metric.Should().Be(_productRequirement.Metric);
        productCategory!.Requirements.First().Min.Should().Be(_productRequirement.Min);
        productCategory!.Requirements.First().Max.Should().Be(_productRequirement.Max);
    }

    [Fact]
    public async Task Handle_WhenProductCategoryAlreadyExistsWithSameExternalId_ShouldThrow()
    {
        var command = new CreateProductCategoryCommand
        {
            ExternalId = Guid.CreateVersion7().ToString()[0..20],
            Name = "Some Category",
            ProductRequirements = [_productRequirement]
        };

        await Sender.Send(command);

        Func<Task> act = async () => await Sender.Send(command);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*A product category with that external id already exists");
    }
}