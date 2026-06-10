using FluentAssertions;
using LogiPulse.Application.ProductCategories.Queries.ListProductCategories;
using LogiPulse.Application.Products.Queries.ListProducts;
using LogiPulse.Domain.Entities.Products;
using Logipulse.IntegrationTests.Setup;
using NSubstitute;

namespace LogiPulse.IntegrationTests.ProductCategories.Queries.ListProductCategories;

public class ListProductCategoriesQueryHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_ShouldReturnProductCategoryItems()
    {
        var productCategory = ProductCategory.Create(UserContext.TenantId, Guid.NewGuid().ToString()[0..8],
            Guid.NewGuid().ToString()[0..8]);

        await DbContext.ProductCategories.AddAsync(productCategory);
        await DbContext.SaveChangesAsync();

        var query = new ListProductCategoriesQuery(productCategory.Name);

        var result = await Sender.Send(query);

        result.Should().BeOfType<List<ListProductCategoriesItemResponse>>();

        result.Should().HaveCount(1);

        result[0].Id.Should().Be(productCategory.Id);
        result[0].ExternalId.Should().Be(productCategory.ExternalId);
        result[0].Name.Should().Be(productCategory.Name);
    }
}