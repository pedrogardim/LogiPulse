using LogiPulse.Domain.Base;

namespace LogiPulse.Domain.Entities.Products;

public class ProductCategory : ProductRuleStore
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
    
    public static ProductCategory Create(string externalId, string name)
    {
        var id = Guid.CreateVersion7();
        var tenantId = Guid.CreateVersion7(); // TODO: Tenant Logic

        return new ProductCategory(id, tenantId, externalId, name);
    }
}