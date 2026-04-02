using LogiPulse.Domain.Base;

namespace LogiPulse.Domain.Entities.Product;

public abstract class ProductRuleStore : Entity
{
    private List<ProductRule> _rules = new();
    public IReadOnlyCollection<ProductRule> Rules => _rules.AsReadOnly();

    protected ProductRuleStore()
    {
    }

    protected ProductRuleStore(Guid id) : base(id)
    {
    }

    public void SetRule(string metricCode, string unitCode, decimal? min, decimal? max)
    {
        _rules.RemoveAll(r => r.MetricCode.Code == metricCode);
        var metric = new ProductRuleMetric(metricCode);
        var unit = new ProductRuleUnit(unitCode);
        
        _rules.Add(new ProductRule(metric, unit, min, max));
    }

    public void ClearRules()
    {
        _rules.Clear();
    }
}