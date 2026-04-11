namespace LogiPulse.Domain.Entities.Products;

public class Product : ProductRuleStore
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

    public static Product Create(string externalId, string code, string name, Guid? categoryId = null)
    {
        var id = Guid.CreateVersion7();
        var tenantId = Guid.CreateVersion7(); // TODO: Tenant Logic

        var product = new Product(id, tenantId, externalId, code, name, categoryId);

        product.SetRule("TEMP", "C", 2, 3.4444m);
        
        return product;
    }
}