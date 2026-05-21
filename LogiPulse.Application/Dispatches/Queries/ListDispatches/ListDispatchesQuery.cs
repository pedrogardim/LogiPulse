using MediatR;

namespace LogiPulse.Application.Dispatches.Queries.ListDispatches;

public record ListDispatchesQuery(
    string? Search = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<IReadOnlyList<ListDispatchesItemResponse>>;