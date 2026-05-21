using LogiPulse.Domain.Entities.Dispatches;
using MediatR;

namespace LogiPulse.Application.Dispatches.Queries.ListDispatches;

public class ListDispatchesQueryHandler(IDispatchRepository dispatchRepository)
    : IRequestHandler<ListDispatchesQuery, IReadOnlyList<ListDispatchesItemResponse>>
{
    public async Task<IReadOnlyList<ListDispatchesItemResponse>> Handle(ListDispatchesQuery query,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("ListDispatchesQuery not implemented");
    }
}