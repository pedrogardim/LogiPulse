namespace LogiPulse.Domain.Entities.Vehicles;

using LogiPulse.Domain.Shared.Transport;

public record VehicleCapability(
    string MetricCode,
    string Unit,
    decimal? Min,
    decimal? Max,
    bool IsAvailable
)
{
    public TransportMetric Metric => new(MetricCode);
    public TransportUnit RuleUnit => new(Unit);
};