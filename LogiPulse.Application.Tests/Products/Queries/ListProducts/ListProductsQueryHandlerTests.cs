using FluentAssertions;
using LogiPulse.Application.Products.Queries.ListProducts;
using LogiPulse.Domain.Entities.Products;
using NSubstitute;

namespace LogiPulse.Application.Tests.Products.Queries.ListProducts;

public class ListProductsQueryHandlerTests
{
    private readonly IProductRepository _productRepositoryMock;
    private readonly ListProductsQueryHandler _handler;
    private readonly ListProductsQuery _query;
    private readonly Guid _productCategoryId = Guid.CreateVersion7();

    public ListProductsQueryHandlerTests()
    {
        _productRepositoryMock = Substitute.For<IProductRepository>();

        _handler = new ListProductsQueryHandler(_productRepositoryMock);

        _query = new ListProductsQuery(
            _productCategoryId,
            "Some Product",
            1,
            20
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnProductItems()
    {
        var product = Product.Create(Guid.CreateVersion7(), "D-01", "3791", "Product", _productCategoryId);

        IReadOnlyList<Product> facilities = [product];

        _productRepositoryMock.ListAsync(
                _productCategoryId,
                _query.Search,
                _query.Page,
                _query.PageSize,
                Arg.Any<CancellationToken>()
            )
            .Returns(facilities);

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().BeOfType<List<ListProductsItemResponse>>();

        result.Should().HaveCount(1);

        result[0].Id.Should().Be(product.Id);
        result[0].ExternalId.Should().Be(product.ExternalId);
        result[0].Name.Should().Be(product.Name);

        await _productRepositoryMock.Received(1).ListAsync(
            _productCategoryId,
            _query.Search,
            _query.Page,
            _query.PageSize,
            Arg.Any<CancellationToken>()
        );
    }
}