namespace LogiPulse.Application.Drivers.DTOs;

public record CreateDriverRequest(
    string ExternalId,
    Guid? UserId,
    string Name,
    string Phone,
    string LicenseNumber,
    DateOnly LicenseExpiryDate
);