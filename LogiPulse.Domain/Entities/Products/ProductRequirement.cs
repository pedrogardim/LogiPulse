using LogiPulse.Domain.Shared.Transport;

namespace LogiPulse.Domain.Entities.Products;

public record ProductRequirement(
    string MetricCode,
    string Unit,
    decimal? Min,
    decimal? Max
)
{
    public TransportMetric Metric => new(MetricCode);
    public TransportUnit RuleUnit => new(Unit);
};