using FluentAssertions;
using LogiPulse.Application.ProductCategories.Commands.DeleteProductCategory;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.ProductCategories.Commands.DeleteProductCategory;

public class DeleteProductCategoryCommandHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteProductCategoryAndCommits()
    {
        var productCategory = ProductCategory.Create(
            UserContext.TenantId,
            Guid.NewGuid().ToString()[0..8],
            "Some Category");

        await DbContext.ProductCategories.AddAsync(productCategory);
        await DbContext.SaveChangesAsync();

        var command = new DeleteProductCategoryCommand(productCategory.Id);
        await Sender.Send(command);

        var result = await DbContext.ProductCategories.FirstOrDefaultAsync(p => p.Id == productCategory.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenProductCategoryDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        var act = async () => await Sender.Send(new DeleteProductCategoryCommand(id));

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Product category don't exist*");
    }
}