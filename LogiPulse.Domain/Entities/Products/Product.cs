using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Entities.Products;

public class Product : ProductRequirementStore
{
    public Guid TenantId { get; private set; }
    public string ExternalId { get; private set; }

    public Guid? CategoryId { get; private set; }
    public virtual ProductCategory? Category { get; private set; }

    public string Name { get; private set; }
    public string Code { get; private set; }

    protected Product()
    {
    }

    private Product(
        Guid id,
        Guid tenantId,
        string externalId, 
        string code, 
        string name, 
        Guid? categoryId = null
    ) : base(id)
    {
        if(tenantId == Guid.Empty)
            throw new BusinessRuleException("TenantId is mandatory");
        
        if (string.IsNullOrWhiteSpace(externalId))
            throw new BusinessRuleException("ExternalId is mandatory");
        
        if (string.IsNullOrWhiteSpace(code))
            throw new BusinessRuleException("Code is mandatory");
        
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleException("Name is mandatory");
        
        TenantId = tenantId;
        ExternalId = externalId;
        Code = code;
        Name = name;
        CategoryId = categoryId;
    }

    public static Product Create(Guid tenantId, string externalId, string code, string name, Guid? productCategoryId = null)
    {
        var id = Guid.CreateVersion7();
        var product = new Product(id, tenantId, externalId, code, name, productCategoryId);
        
        return product;
    }
}