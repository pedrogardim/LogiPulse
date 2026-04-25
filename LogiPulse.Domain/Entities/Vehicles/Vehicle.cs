using LogiPulse.Domain.Base;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Facilities;

namespace LogiPulse.Domain.Entities.Vehicles;

public class Vehicle : Entity
{
    public Guid TenantId { get; private set; }

    public string ExternalId { get; private set; }

    public string Name { get; private set; }
    public string LicensePlate { get; private set; }
    public bool IsActive { get; private set; }
    
    private List<VehicleCapability> _capabilities = new();
    public IReadOnlyCollection<VehicleCapability> Capabilities => _capabilities.AsReadOnly();
    
    public VehicleType Type { get; private set; }

    public Guid? HomeFacilityId { get; private set; }
    public virtual Facility? HomeFacility { get; private set; }

    public virtual List<Dispatch> Dispatches { get; } = [];
    
    
    protected Vehicle()
    {
    }

    private Vehicle(Guid id, Guid tenantId, string externalId, string name, string licensePlate, VehicleType type) : base(id)
    {
        TenantId = tenantId;
        ExternalId = externalId;
        Name = name;
        LicensePlate = licensePlate;
        Type = type;
    }

    public static Vehicle Create(
        Guid tenantId,
        string externalId,
        string name,
        string licensePlate,
        VehicleType type)
    {
        var id = Guid.CreateVersion7();
        var vehicle = new Vehicle(id, tenantId, externalId, name, licensePlate, type);
        return vehicle;
    }
    
    public void SetCapability(string metricCode, string unitCode, decimal? min, decimal? max)
    {
        _capabilities.RemoveAll(r => r.MetricCode == metricCode);
        _capabilities.Add(new VehicleCapability(metricCode, unitCode, min, max, true));
    }
    
    public void Activate() => IsActive = true;

    public void AssignToFacility(Guid facilityId) => HomeFacilityId = facilityId;
    
    public void UnassignFacility() => HomeFacilityId = null;
    
}