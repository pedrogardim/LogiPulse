using FluentAssertions;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Tests.Entities.Drivers;

public class DriverTests
{
    private const string ExternalId = "D001";
    private const string Name = "John Doe";
    private const string Phone = "123-456-7890";
    private const string LicenseNumber = "ABC-7890";

    private Guid _tenantId = Guid.NewGuid();
    private Guid _userId = Guid.NewGuid();
    private DateOnly _licenseExpiryDate = new(2030, 1, 1);

    [Fact]
    public void Create_ReturnsDriver()
    {
        var driver = Driver.Create(_tenantId, _userId, ExternalId, Name, Phone, LicenseNumber, _licenseExpiryDate);
        driver.Should().NotBeNull();

        driver.TenantId.Should().Be(_tenantId);
        driver.UserId.Should().Be(_userId);
        driver.ExternalId.Should().Be(ExternalId);
        driver.LicenseNumber.Should().Be(LicenseNumber);
        driver.LicenseExpiryDate.Should().Be(_licenseExpiryDate);
        driver.Phone.Should().Be(Phone);

        driver.IsActive.Should().Be(true);
    }

    [Fact]
    public void Create_InvalidTenantId_ThrowsException()
    {
        Action act = () =>
            Driver.Create(Guid.Empty, _userId, ExternalId, Name, Phone, LicenseNumber, _licenseExpiryDate);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("TenantId is mandatory");
    }

    [Fact]
    public void Create_InvalidUserId_ThrowsException()
    {
        Action act = () =>
            Driver.Create(_tenantId, Guid.Empty, ExternalId, Name, Phone, LicenseNumber, _licenseExpiryDate);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("UserId must be null or a non-empty Guid");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidExternalId_ThrowsException(string? externalId)
    {
        Action act = () =>
            Driver.Create(_tenantId, _userId, externalId!, Name, Phone, LicenseNumber, _licenseExpiryDate);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("ExternalId is mandatory");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidName_ThrowsException(string? name)
    {
        Action act = () =>
            Driver.Create(_tenantId, _userId, ExternalId, name!, Phone, LicenseNumber, _licenseExpiryDate);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Name is mandatory");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidPhone_ThrowsException(string? phone)
    {
        Action act = () =>
            Driver.Create(_tenantId, _userId, ExternalId, Name, phone!, LicenseNumber, _licenseExpiryDate);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Phone is mandatory");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidLicenseNumber_ThrowsException(string? licenseNumber)
    {
        Action act = () =>
            Driver.Create(_tenantId, _userId, ExternalId, Name, Phone, licenseNumber!, _licenseExpiryDate);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("LicenseNumber is mandatory");
    }

    [Fact]
    public void Create_ExpiredDriverLicense_ThrowsException()
    {
        var expiredDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        Action act = () => Driver.Create(_tenantId, _userId, ExternalId, Name, Phone, LicenseNumber, expiredDate);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Driver License is expired");
    }
}