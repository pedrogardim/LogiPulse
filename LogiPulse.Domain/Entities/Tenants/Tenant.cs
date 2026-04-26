using LogiPulse.Domain.Base;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Entities.Tenants;

public class Tenant : Entity
{
    public string DisplayName { get; private set; }
    
    public string TaxCode { get; private set; }
    
    public string? LegalName { get; private set; }
    
    protected Tenant()
    {
    }
    
    private Tenant(Guid id, string displayName, string taxCode, string? legalName = null) : base(id)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new BusinessRuleException("DisplayName is mandatory");
        
        if (string.IsNullOrWhiteSpace(taxCode))
            throw new BusinessRuleException("TaxCode is mandatory");
        
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