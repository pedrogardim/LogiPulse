using Xunit;
using FluentAssertions;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;

namespace LogiPulse.Domain.Tests.Entities.Users;

public class UserTests
{
    private readonly string _validFullName = "Test User";
    private readonly Email _validEmail = Email.Create("test@mail.com");

    [Fact]
    public void Create_WithEntraId_ReturnsUser()
    {
        ;
        var tenantId = Guid.NewGuid();
        var entraId = Guid.NewGuid();

        var user = User.Create(tenantId, _validEmail, _validFullName, entraId);
        user.Should().NotBeNull();

        user.TenantId.Should().Be(tenantId);
        user.Email.Should().Be(_validEmail);
        user.FullName.Should().Be(_validFullName);
        user.EntraId.Should().Be(entraId);
    }

    [Fact]
    public void Create_WithoutEntraId_ReturnsUser()
    {
        ;
        var tenantId = Guid.NewGuid();

        var user = User.Create(tenantId, _validEmail, _validFullName);
        user.Should().NotBeNull();

        user.TenantId.Should().Be(tenantId);
        user.Email.Should().Be(_validEmail);
        user.FullName.Should().Be(_validFullName);
        user.EntraId.Should().BeNull();
    }


    [Fact]
    public void Create_InvalidTenantId_ThrowsException()
    {
        Action act = () => User.Create(Guid.Empty, _validEmail, _validFullName);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("TenantId is mandatory");
    }

    [Fact]
    public void Create_InvalidFullname_ThrowsException()
    {
        Action act = () => User.Create(Guid.NewGuid(), _validEmail, "");

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("FullName is mandatory");
    }

    [Fact]
    public void Change_EntraID_Throws_Exception()
    {
        var tenantId = Guid.NewGuid();
        var entraId = Guid.NewGuid();

        var user = User.Create(tenantId, _validEmail, _validFullName, entraId);

        user.Invoking(u => u.SetEntraId(Guid.NewGuid()))
            .Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Azure Entra ID can only be set once");
    }
}