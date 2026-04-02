namespace LogiPulse.Domain.Entities.Product;

public record ProductRule(
    ProductRuleMetric MetricCode,
    ProductRuleUnit Unit,
    decimal? Min,
    decimal? Max
);