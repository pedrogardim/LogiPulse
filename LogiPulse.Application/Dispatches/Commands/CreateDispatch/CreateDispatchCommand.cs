using MediatR;

namespace LogiPulse.Application.Dispatches.Commands.CreateDispatch;

public record CreateDispatchCommand : IRequest<Guid>
{
    public string ExternalId;
    public Guid ProductId;
    public Guid OriginFacilityId;
    public Guid DestinationFacilityId;
    public Guid? VehicleId;
    public Guid? DriverId;
}