using FluentAssertions;
using LogiPulse.Application.Products.Commands.DeleteProduct;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteProductAndCommits()
    {
        var product = Product.Create(UserContext.TenantId, "D-01", "3791", "Product");

        await DbContext.Products.AddAsync(product);
        await DbContext.SaveChangesAsync();

        var command = new DeleteProductCommand(product.Id);
        await Sender.Send(command);

        var result = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenProductDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        var act = async () => await Sender.Send(new DeleteProductCommand(id));

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Product don't exist*");
    }
}