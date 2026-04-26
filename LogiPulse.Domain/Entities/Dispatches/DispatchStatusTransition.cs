namespace LogiPulse.Domain.Entities.Dispatches;

public record DispatchStatusTransition(
    DispatchStatus Status,
    DateTime OccurredAtUtc,
    string? Reason = null
);