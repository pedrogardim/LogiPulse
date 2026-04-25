using LogiPulse.Domain.Entities.Tenants;

namespace LogiPulse.Domain.Entities.Products;

public class Product : ProductRequirementStore
{
    public Guid TenantId { get; private set; }
    public string ExternalId { get; private set; }

    public Guid? CategoryId { get; private set; }
    public virtual ProductCategory? Category { get; private set; }

    public string Name { get; private set; }
    public string Code { get; private set; }

    public Product()
    {
    }

    private Product(
        Guid id,
        Guid tenantId,
        string externalId, 
        string code, 
        string name, 
        Guid? categoryId
    ) : base(id)
    {
        TenantId = tenantId;
        ExternalId = externalId;
        Code = code;
        Name = name;
        CategoryId = categoryId;
    }

    public static Product Create(Tenant tenant, string externalId, string code, string name, ProductCategory? category = null)
    {
        var id = Guid.CreateVersion7();
        var product = new Product(id, tenant.Id, externalId, code, name, category?.Id);
        
        return product;
    }
}