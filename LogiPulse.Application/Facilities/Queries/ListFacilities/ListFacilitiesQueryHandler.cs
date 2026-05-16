using LogiPulse.Domain.Entities.Facilities;
using MediatR;

namespace LogiPulse.Application.Facilities.Queries.ListFacilities;

public class ListFacilitiesQueryHandler(IFacilityRepository facilityRepository)
    : IRequestHandler<ListFacilitiesQuery, IReadOnlyList<ListFacilitiesItemResponse>>
{
    public async Task<IReadOnlyList<ListFacilitiesItemResponse>> Handle(ListFacilitiesQuery query,
        CancellationToken cancellationToken)
    {
        var facilities =
            await facilityRepository.ListAsync(
                query.FacilityType,
                query.Search,
                query.Page,
                query.PageSize,
                cancellationToken);

        return facilities.Select(f => new ListFacilitiesItemResponse(
            f.Id,
            f.ExternalId,
            f.Name,
            f.Code,
            f.Type,
            f.Address.City,
            f.Address.State
        )).ToList();
    }
}