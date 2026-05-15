using FluentAssertions;
using LogiPulse.Application.Drivers.Commands;

namespace LogiPulse.Application.Tests.Drivers.Commands;

public class CreateDriverCommandValidatorTests
{
    [Fact]
    public async Task Validator_Should_Fail_For_Default_Guid_And_Date()
    {
        var command = new CreateDriverCommand
        {
            TenantId = Guid.CreateVersion7(),
            ExternalId = "D-00001",
            UserId = Guid.Empty,
            Name = "Pedro",
            Phone = "12345678",
            LicenseNumber = "8310XY",
            LicenseExpiryDate = DateOnly.MinValue
        };

        var validator = new CreateDriverCommandValidator();
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();

        result.Errors
            .Should()
            .Contain(x =>
                x.PropertyName == nameof(CreateDriverCommand.UserId) &&
                x.ErrorMessage.Contains("UserId"));

        result.Errors
            .Should()
            .Contain(x =>
                x.PropertyName == nameof(CreateDriverCommand.LicenseExpiryDate) &&
                x.ErrorMessage.Contains("LicenseExpiryDate"));
    }
}