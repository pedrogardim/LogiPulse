using FluentAssertions;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Tests.Entities.Tenants;

public class TenantTests
{
    private const string ValidDisplayName = "Logipulse";
    private const string ValidTaxCode = "12345678X";
    private const string ValidLegalName = "LOGIPULSE SOFTWARE LLC";
    
    [Theory]
    [InlineData(ValidDisplayName, ValidTaxCode, ValidLegalName)]
    [InlineData(ValidDisplayName, ValidTaxCode, null)]
    public void Create_ReturnsTenant(string displayName, string taxCode, string? legalName)
    {
        var tenant = Tenant.Create(displayName, taxCode, legalName);
        tenant.Should().NotBeNull();

        tenant.DisplayName.Should().Be(displayName);
        tenant.TaxCode.Should().Be(taxCode);
        tenant.LegalName.Should().Be(legalName ?? null);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidDisplayName_ThrowsException(string displayName)
    {
        Action act = () => Tenant.Create(displayName, ValidTaxCode);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("DisplayName is mandatory");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidTaxCode_ThrowsException(string taxCode)
    {
        Action act = () => Tenant.Create(ValidDisplayName, taxCode);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("TaxCode is mandatory");
    }
}