using LogiPulse.Domain.Entities.Facilities;
using MediatR;

namespace LogiPulse.Application.Facilities.Queries.ListFacilities;

public record ListFacilitiesQuery(
    FacilityType? FacilityType,
    string? Search = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<IReadOnlyList<ListFacilitiesItemResponse>>;