using FluentAssertions;
using LogiPulse.Application.Products.Commands.UpdateProduct;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_WhenValid_UpdatesProduct()
    {
        var product = Product.Create(UserContext.TenantId, Guid.CreateVersion7().ToString()[0..15], "3791", "Product");

        await DbContext.Products.AddAsync(product);
        await DbContext.SaveChangesAsync();

        var command = new UpdateProductCommand
        {
            Id = product.Id,
            Name = "Westroot Warehouse",
            Code = "12345"
        };

        await Sender.Send(command);

        var retrievedProduct = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

        retrievedProduct.Should().NotBeNull();
        retrievedProduct!.Name.Should().Be(command.Name);
        retrievedProduct!.Code.Should().Be(command.Code);
    }

    [Fact]
    public async Task Handle_WhenProductDontExist_ShouldThrow()
    {
        var command = new UpdateProductCommand
        {
            Id = Guid.CreateVersion7(),
            Name = "Westroot Warehouse"
        };

        Func<Task> act = async () => await Sender.Send(command);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Product not found*");
    }
}