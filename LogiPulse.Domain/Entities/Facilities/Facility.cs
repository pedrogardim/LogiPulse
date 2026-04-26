using LogiPulse.Domain.Base;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using NetTopologySuite.Geometries;

namespace LogiPulse.Domain.Entities.Facilities;

public class Facility : Entity
{
    public Guid TenantId { get; private set; }

    public string ExternalId { get; private set; }

    public string Name { get; private set; }
    public string Code { get; private set; }
    public FacilityType Type { get; private set; }

    public Point Location { get; private set; }
    public Address Address { get; private set; }

    protected Facility()
    {
    }

    private Facility(
        Guid id,
        Guid tenantId,
        string externalId,
        string name,
        string code,
        FacilityType type,
        Address address,
        Point location) : base(id)
    {
        if(tenantId == Guid.Empty)
            throw new BusinessRuleException("TenantId is mandatory");
        
        if (string.IsNullOrWhiteSpace(externalId))
            throw new BusinessRuleException("ExternalId is mandatory");
        
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleException("Name is mandatory");
        
        if (string.IsNullOrWhiteSpace(code))
            throw new BusinessRuleException("Code is mandatory");
        
        TenantId = tenantId;
        ExternalId = externalId;
        Name = name;
        Code = code;
        Type = type;
        Address = address;
        Location = location;
    }

    public static Facility Create(
        Guid tenantId,
        string externalId,
        string name,
        string code,
        FacilityType type,
        Address address,
        Point location)
    {
        var id = Guid.CreateVersion7();
        var facility = new Facility(id, tenantId, externalId, name, code, type, address, location);
        return facility;
    }
}