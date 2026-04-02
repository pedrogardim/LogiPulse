namespace LogiPulse.Domain.Entities.Product;

public record ProductRule(
    string MetricCode,
    string Unit,
    decimal? Min,
    decimal? Max
)
{
    public ProductRuleMetric Metric => new(MetricCode);
    public ProductRuleUnit RuleUnit => new(Unit);
};