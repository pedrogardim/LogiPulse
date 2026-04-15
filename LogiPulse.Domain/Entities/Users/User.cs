using LogiPulse.Domain.Base;
using LogiPulse.Domain.Entities.Tenants;

namespace LogiPulse.Domain.Entities.Users;

public class User : Entity
{
    public Guid TenantId { get; private set; }
    public virtual Tenant Tenant { get; private set; }
    
    public string Email { get; private set; }
    public string FullName { get; private set; }

    // For external authentication with Azure Entra
    public Guid? EntraId { get; private set; }

    protected User()
    {
    }

    private User(Guid id, Guid tenantId, string email, string fullName, Guid? entraId) : base(id)
    {
        TenantId = tenantId;
        Email = email;
        FullName = fullName;
        EntraId = entraId;
    }

    public static User Create(Guid tenantId, string email, string fullName, Guid? entraId)
    {
        var id = Guid.CreateVersion7();
        var user = new User(id, tenantId, email, fullName, entraId);
        return user;
    }

    public User SetEntraId(Guid entraId)
    {
        if (EntraId != null)
            throw new Exception("Azure Entra ID can only be set once");

        EntraId = entraId;
        return this;
    }
}