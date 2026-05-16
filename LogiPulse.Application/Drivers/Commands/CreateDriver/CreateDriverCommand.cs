using MediatR;

namespace LogiPulse.Application.Drivers.Commands.CreateDriver;

public record CreateDriverCommand : IRequest<Guid>
{
    public string ExternalId;
    public Guid? UserId;
    public string Name;
    public string Phone;
    public string LicenseNumber;
    public DateOnly LicenseExpiryDate;
}