using FluentAssertions;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;

namespace LogiPulse.Domain.Tests.Entities.Users;

public class UserTests
{
    private const string ValidFullName = "Test User";
    private readonly Email _validEmail = Email.Create("test@mail.com");

    [Fact]
    public void Create_WithEntraId_ReturnsUser()
    {
        var tenantId = Guid.NewGuid();
        var entraId = Guid.NewGuid();

        var user = User.Create(tenantId, _validEmail, ValidFullName, entraId);
        user.Should().NotBeNull();

        user.TenantId.Should().Be(tenantId);
        user.Email.Should().Be(_validEmail);
        user.FullName.Should().Be(ValidFullName);
        user.EntraId.Should().Be(entraId);
    }

    [Fact]
    public void Create_WithoutEntraId_ReturnsUser()
    {
        var tenantId = Guid.NewGuid();

        var user = User.Create(tenantId, _validEmail, ValidFullName);
        user.Should().NotBeNull();

        user.TenantId.Should().Be(tenantId);
        user.Email.Should().Be(_validEmail);
        user.FullName.Should().Be(ValidFullName);
        user.EntraId.Should().BeNull();
    }


    [Fact]
    public void Create_InvalidTenantId_ThrowsException()
    {
        Action act = () => User.Create(Guid.Empty, _validEmail, ValidFullName);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("TenantId is mandatory");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidFullname_ThrowsException(string? fullName)
    {
        Action act = () => User.Create(Guid.NewGuid(), _validEmail, fullName!);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("FullName is mandatory");
    }

    [Fact]
    public void Change_EntraID_Throws_Exception()
    {
        var tenantId = Guid.NewGuid();
        var entraId = Guid.NewGuid();

        var user = User.Create(tenantId, _validEmail, ValidFullName, entraId);

        user.Invoking(u => u.SetEntraId(Guid.NewGuid()))
            .Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Azure Entra ID can only be set once");
    }
}