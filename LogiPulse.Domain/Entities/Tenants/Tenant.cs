using LogiPulse.Domain.Base;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Users;

namespace LogiPulse.Domain.Entities.Tenants;

public class Tenant : Entity
{
    public string DisplayName { get; private set; }
    
    public string TaxCode { get; private set; }
    
    public string? LegalName { get; private set; }

    public virtual List<Dispatch> Dispatches { get; } = [];
    public virtual List<Product> Products { get; } = [];
    public virtual List<ProductCategory> ProductCategories { get; } = [];
    public virtual List<User> Users { get; } = [];
    public virtual List<Facility> Facilities { get; } = [];
    
    protected Tenant()
    {
    }
    
    private Tenant(Guid id, string displayName, string taxCode, string? legalName) : base(id)
    {
        DisplayName = displayName;
        TaxCode = taxCode;
        LegalName = legalName;
    }
    
    public static Tenant Create(string displayName, string taxCode, string? legalName = null)
    {
        var id = Guid.CreateVersion7();
        var tenant = new Tenant(id, displayName, taxCode, legalName);
        return tenant;
    }

}