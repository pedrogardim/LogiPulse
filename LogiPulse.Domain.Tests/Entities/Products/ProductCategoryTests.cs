using FluentAssertions;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Tests.Entities.Products;

public class ProductCategoryTests
{
    private const string ExternalId = "VAC";
    private const string Name = "Vaccines";

    [Fact]
    public void Create_ReturnsProductCategory()
    {
        var tenantId = Guid.NewGuid();

        var product = ProductCategory.Create(tenantId, ExternalId, Name);
        product.Should().NotBeNull();

        product.TenantId.Should().Be(tenantId);
        product.ExternalId.Should().Be(ExternalId);
        product.Name.Should().Be(Name);
    }

    [Fact]
    public void Create_InvalidTenantId_ThrowsException()
    {
        Action act = () => ProductCategory.Create(Guid.Empty, ExternalId, Name);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("TenantId is mandatory");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidExternalId_ThrowsException(string? externalId)
    {
        Action act = () => ProductCategory.Create(Guid.NewGuid(), externalId!, Name);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("ExternalId is mandatory");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidName_ThrowsException(string? name)
    {
        Action act = () => ProductCategory.Create(Guid.NewGuid(), ExternalId, name!);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Name is mandatory");
    }

    [Fact]
    public void Update_UpdatesProductCategory()
    {
        var tenantId = Guid.NewGuid();
        var productCategory = ProductCategory.Create(tenantId, ExternalId, "_");

        productCategory.Update(Name);

        productCategory.TenantId.Should().Be(tenantId);
        productCategory.ExternalId.Should().Be(ExternalId);

        productCategory.Name.Should().Be(Name);
    }
}