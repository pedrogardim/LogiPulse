using FluentAssertions;
using LogiPulse.Application.ProductCategories.Queries.ListProductCategories;
using LogiPulse.Domain.Entities.Products;
using NSubstitute;

namespace LogiPulse.Application.Tests.ProductCategories.Queries.ListProductCategories;

public class ListProductCategoriesQueryHandlerTests
{
    private readonly IProductCategoryRepository _productCategoryRepositoryMock;
    private readonly ListProductCategoriesQueryHandler _handler;
    private readonly ListProductCategoriesQuery _query;

    public ListProductCategoriesQueryHandlerTests()
    {
        _productCategoryRepositoryMock = Substitute.For<IProductCategoryRepository>();

        _handler = new ListProductCategoriesQueryHandler(_productCategoryRepositoryMock);

        _query = new ListProductCategoriesQuery(
            "Some ProductCategory",
            1,
            20
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnProductCategoryItems()
    {
        var productCategory = ProductCategory.Create(Guid.CreateVersion7(), "D-01", "ProductCategory");

        IReadOnlyList<ProductCategory> facilities = [productCategory];

        _productCategoryRepositoryMock.ListAsync(
                _query.Search,
                _query.Page,
                _query.PageSize,
                Arg.Any<CancellationToken>()
            )
            .Returns(facilities);

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().BeOfType<List<ListProductCategoriesItemResponse>>();

        result.Should().HaveCount(1);

        result[0].Id.Should().Be(productCategory.Id);
        result[0].ExternalId.Should().Be(productCategory.ExternalId);
        result[0].Name.Should().Be(productCategory.Name);

        await _productCategoryRepositoryMock.Received(1).ListAsync(
            _query.Search,
            _query.Page,
            _query.PageSize,
            Arg.Any<CancellationToken>()
        );
    }
}