using LogiPulse.Domain.Base;

namespace LogiPulse.Domain.Entities.Products;

public abstract class ProductRequirementStore : Entity
{
    private List<ProductRequirement> _requirements = new();
    public IReadOnlyCollection<ProductRequirement> Requirements => _requirements.AsReadOnly();

    protected ProductRequirementStore()
    {
    }

    protected ProductRequirementStore(Guid id) : base(id)
    {
    }

    public void SetRequirement(string metricCode, string unitCode, decimal? min, decimal? max)
    {
        _requirements.RemoveAll(r => r.MetricCode == metricCode);
        _requirements.Add(new ProductRequirement(metricCode, unitCode, min, max));
    }

    public void ClearRequirements()
    {
        _requirements.Clear();
    }
}