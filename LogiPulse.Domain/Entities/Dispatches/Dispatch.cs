using System.Security.Cryptography;
using LogiPulse.Domain.Base;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Vehicles;

namespace LogiPulse.Domain.Entities.Dispatches;

public class Dispatch : Entity
{
    public Guid TenantId { get; private set; }
    
    public string ExternalId { get; private set; }
    
    public Guid ProductId { get; private set; }
    public virtual Product Product { get; private set; }
    
    public DispatchStatus CurrentStatus { get; private set; }

    private readonly List<DispatchStatusTransition> _statusHistory = [];
    public IReadOnlyCollection<DispatchStatusTransition> StatusHistory => _statusHistory.AsReadOnly();
    
    public Guid OriginFacilityId { get; private set; }
    public virtual Facility OriginFacility { get; private set; }
    
    public Guid DestinationFacilityId { get; private set; }
    public virtual Facility DestinationFacility { get; private set; }

    public Guid? VehicleId { get; private set; }
    public virtual Vehicle? Vehicle { get; private set; }
    
    protected Dispatch()
    {
    }

    private Dispatch(Guid id, Guid tenantId, string externalId, Guid productId, Guid originFacilityId, Guid destinationFacilityId, Guid vehicleId) : base(id)
    {
        TenantId = tenantId;
        ExternalId = externalId;
        ProductId = productId;
        VehicleId = vehicleId;
        OriginFacilityId = originFacilityId;
        DestinationFacilityId = destinationFacilityId;
    }

    public static Dispatch Create(string externalId, Guid tenantId, Guid productId, Guid originFacilityId, Guid destinationFacilityId, Guid vehicleId)
    {
        var id = Guid.CreateVersion7();
        var dispatch = new Dispatch(id, tenantId, externalId, productId, originFacilityId, destinationFacilityId, vehicleId);
        dispatch.ChangeStatus(DispatchStatus.Created);
        return dispatch;
    }

    public void ChangeStatus(DispatchStatus newStatus, string? reason = null)
    {
        CurrentStatus = newStatus;
        _statusHistory.Add(new DispatchStatusTransition(newStatus, DateTime.UtcNow, reason));
    }
}