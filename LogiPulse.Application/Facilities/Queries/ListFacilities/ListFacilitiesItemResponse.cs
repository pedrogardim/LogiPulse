using LogiPulse.Domain.Entities.Facilities;

namespace LogiPulse.Application.Facilities.Queries.ListFacilities;

public record ListFacilitiesItemResponse(
    Guid Id,
    string ExternalId,
    string Name,
    string Code,
    FacilityType Type,
    string City,
    string State
);