namespace LogiPulse.Domain.Entities.Dispatches;

public record DispatchStatusTransition(
    DispatchStatus Status,
    DateTime OcurredAtUtc,
    string? Reason = null
);