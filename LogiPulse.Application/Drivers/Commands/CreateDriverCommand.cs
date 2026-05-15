using MediatR;

namespace LogiPulse.Application.Drivers.Commands;

public record CreateDriverCommand : IRequest<Guid>
{
    public Guid TenantId;
    public string ExternalId;
    public Guid? UserId;
    public string Name;
    public string Phone;
    public string LicenseNumber;
    public DateOnly LicenseExpiryDate;
}