using LogiPulse.Domain.Base;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;

namespace LogiPulse.Domain.Entities.Users;

public class User : Entity
{
    public Guid TenantId { get; private set; }
    public virtual Tenant Tenant { get; private set; }
    
    public Email Email { get; private set; }
    public string FullName { get; private set; }

    // For external authentication with Azure Entra
    public Guid? EntraId { get; private set; }

    protected User()
    {
    }

    private User(Guid id, Guid tenantId, Email email, string fullName, Guid? entraId = null) : base(id)
    {
        if(tenantId == Guid.Empty)
            throw new BusinessRuleException("TenantId is mandatory");
        
        if (string.IsNullOrWhiteSpace(fullName))
            throw new BusinessRuleException("FullName is mandatory");
        
        TenantId = tenantId;
        Email = email;
        FullName = fullName;
        EntraId = entraId;
    }

    public static User Create(Guid tenantId, Email email, string fullName, Guid? entraId = null)
    {
        var id = Guid.CreateVersion7();
        var user = new User(id, tenantId, email, fullName, entraId);
        return user;
    }

    public User SetEntraId(Guid entraId)
    {
        if (EntraId != null)
            throw new BusinessRuleException("Azure Entra ID can only be set once");

        EntraId = entraId;
        return this;
    }
}