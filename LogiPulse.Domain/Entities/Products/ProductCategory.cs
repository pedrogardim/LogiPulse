using LogiPulse.Domain.Base;
using LogiPulse.Domain.Entities.Tenants;

namespace LogiPulse.Domain.Entities.Products;

public class ProductCategory : ProductRequirementStore
{
    public Guid TenantId { get; private set; }
    public string ExternalId { get; private set; }

    public string Name { get; private set; }

    public ProductCategory()
    {
    }

    private ProductCategory(Guid id, Guid tenantId, string externalId, string name) : base(id)
    {
        TenantId = tenantId;
        ExternalId = externalId;
        Name = name;
    }
    
    public static ProductCategory Create(Tenant tenant, string externalId, string name)
    {
        var id = Guid.CreateVersion7();
        return new ProductCategory(id, tenant.Id, externalId, name);
    }
}