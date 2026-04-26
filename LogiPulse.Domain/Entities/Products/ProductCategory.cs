using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Entities.Products;

public class ProductCategory : ProductRequirementStore
{
    public Guid TenantId { get; private set; }
    public string ExternalId { get; private set; }

    public string Name { get; private set; }

    protected ProductCategory()
    {
    }

    private ProductCategory(Guid id, Guid tenantId, string externalId, string name) : base(id)
    {
        if (tenantId == Guid.Empty)
            throw new BusinessRuleException("TenantId is mandatory");

        if (string.IsNullOrWhiteSpace(externalId))
            throw new BusinessRuleException("ExternalId is mandatory");

        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleException("Name is mandatory");

        TenantId = tenantId;
        ExternalId = externalId;
        Name = name;
    }

    public static ProductCategory Create(Guid tenantId, string externalId, string name)
    {
        var id = Guid.CreateVersion7();
        return new ProductCategory(id, tenantId, externalId, name);
    }
}