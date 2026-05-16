using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Shared;
using MediatR;
using NetTopologySuite.Geometries;

namespace LogiPulse.Application.Facilities.Commands.CreateFacility;

public record CreateFacilityCommand : IRequest<Guid>
{
    public Guid TenantId;
    public string ExternalId;
    public string Name;
    public string Code;
    public FacilityType FacilityType;
    public double Latitude;
    public double Longitude;
    public Address Address;
}