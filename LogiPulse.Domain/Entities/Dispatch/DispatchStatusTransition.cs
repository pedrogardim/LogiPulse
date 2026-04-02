namespace LogiPulse.Domain.Entities.Dispatch;

public record DispatchStatusTransition(
    DispatchStatus Status,
    DateTime OcurredAtUtc,
    string? Reason = null // More details
);