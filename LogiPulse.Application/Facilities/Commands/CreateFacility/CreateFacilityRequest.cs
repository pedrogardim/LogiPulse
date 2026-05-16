using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Shared;
using NetTopologySuite.Geometries;

namespace LogiPulse.Application.Facilities.Commands.CreateFacility;

public record CreateFacilityRequest(
    string ExternalId,
    string Name,
    string Code,
    FacilityType FacilityType,
    double Latitude,
    double Longitude,
    Address Address
);