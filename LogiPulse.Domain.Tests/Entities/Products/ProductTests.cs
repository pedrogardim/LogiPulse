using FluentAssertions;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Tests.Entities.Products;

public class ProductTests
{
    private const string ExternalId = "REF-PFIZER-01";
    private const string Code = "VAC-PFIZER";
    private const string Name = "Pfizer Vaccine";
    
    [Fact]
    public void Create_ReturnsProduct()
    {
        var tenantId = Guid.NewGuid();

        var product = Product.Create(tenantId, ExternalId, Code, Name);
        product.Should().NotBeNull();

        product.TenantId.Should().Be(tenantId);
        product.ExternalId.Should().Be(ExternalId);
        product.Code.Should().Be(Code);
        product.Name.Should().Be(Name);
    }
    
    [Fact]
    public void Create_ReturnsProduct_WithProductCategoryId()
    {
        var tenantId = Guid.NewGuid();
        var productCategoryId = Guid.NewGuid();

        var product = Product.Create(tenantId, ExternalId, Code, Name, productCategoryId);
        product.Should().NotBeNull();

        product.TenantId.Should().Be(tenantId);
        product.ExternalId.Should().Be(ExternalId);
        product.Code.Should().Be(Code);
        product.Name.Should().Be(Name);
        product.CategoryId.Should().Be(productCategoryId);
    }
    
    [Fact]
    public void Create_InvalidTenantId_ThrowsException()
    {
        Action act = () => Product.Create(Guid.Empty, ExternalId, Code, Name);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("TenantId is mandatory");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidExternalId_ThrowsException(string? externalId)
    {
        Action act = () => Product.Create(Guid.NewGuid(), externalId!, Code, Name);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("ExternalId is mandatory");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidCode_ThrowsException(string? code)
    {
        Action act = () => Product.Create(Guid.NewGuid(), ExternalId, code!, Name);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Code is mandatory");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidName_ThrowsException(string? name)
    {
        Action act = () => Product.Create(Guid.NewGuid(), ExternalId, Code, name!);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Name is mandatory");
    }
}