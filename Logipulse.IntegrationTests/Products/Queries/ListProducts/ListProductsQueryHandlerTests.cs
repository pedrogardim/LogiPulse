using FluentAssertions;
using LogiPulse.Application.Products.Queries.ListProducts;
using LogiPulse.Domain.Entities.Products;
using Logipulse.IntegrationTests.Setup;

namespace LogiPulse.IntegrationTests.Products.Queries.ListProducts;

public class ListProductsQueryHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_ShouldReturnProductItems()
    {
        var productCategory = ProductCategory.Create(UserContext.TenantId, "VCN", "Vaccines");
        var product = Product.Create(UserContext.TenantId, Guid.CreateVersion7().ToString()[0..15], "3791", "Product",
            productCategory.Id);

        await DbContext.ProductCategories.AddAsync(productCategory);
        await DbContext.Products.AddAsync(product);
        await DbContext.SaveChangesAsync();

        var query = new ListProductsQuery(
            productCategory.Id,
            "Product"
        );

        var result = await Sender.Send(query, CancellationToken.None);

        result.Should().BeOfType<List<ListProductsItemResponse>>();

        result.Should().HaveCount(1);

        result[0].Id.Should().Be(product.Id);
        result[0].ExternalId.Should().Be(product.ExternalId);
        result[0].Name.Should().Be(product.Name);
    }
}