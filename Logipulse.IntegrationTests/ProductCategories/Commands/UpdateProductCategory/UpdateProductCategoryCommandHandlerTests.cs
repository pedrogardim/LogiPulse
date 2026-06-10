using FluentAssertions;
using LogiPulse.Application.ProductCategories.Commands.UpdateProductCategory;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.ProductCategories.Commands.UpdateProductCategory;

public class UpdateProductCategoryCommandHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_WhenValid_UpdatesProductCategory()
    {
        var productCategory = ProductCategory.Create(
            UserContext.TenantId,
            Guid.CreateVersion7().ToString()[0..15],
            Guid.CreateVersion7().ToString()[0..8]);

        await DbContext.ProductCategories.AddAsync(productCategory);
        await DbContext.SaveChangesAsync();

        var command = new UpdateProductCategoryCommand
        {
            Id = productCategory.Id,
            Name = "Some other category"
        };

        await Sender.Send(command);

        var retrievedProductCategory =
            await DbContext.ProductCategories.FirstOrDefaultAsync(p => p.Id == productCategory.Id);

        retrievedProductCategory.Should().NotBeNull();
        retrievedProductCategory!.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Handle_WhenProductCategoryDontExist_ShouldThrow()
    {
        var command = new UpdateProductCategoryCommand
        {
            Id = Guid.CreateVersion7(),
            Name = "Some other category"
        };

        Func<Task> act = async () => await Sender.Send(command);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Product category not found*");
    }
}