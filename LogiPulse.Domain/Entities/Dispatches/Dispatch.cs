using System.Security.Cryptography;
using LogiPulse.Domain.Base;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Entities.Dispatches;

public class Dispatch : Entity
{
    public Guid TenantId { get; private set; }

    public string ExternalId { get; private set; }

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }

    public DispatchStatus CurrentStatus { get; private set; }

    private readonly List<DispatchStatusTransition> _statusHistory = [];
    public IReadOnlyCollection<DispatchStatusTransition> StatusHistory => _statusHistory.AsReadOnly();

    public Guid OriginFacilityId { get; private set; }
    public Facility OriginFacility { get; private set; }

    public Guid DestinationFacilityId { get; private set; }
    public Facility DestinationFacility { get; private set; }

    public Guid? VehicleId { get; private set; }
    public Vehicle? Vehicle { get; private set; }

    public Guid? DriverId { get; private set; }
    public Driver? Driver { get; private set; }

    protected Dispatch()
    {
    }

    private Dispatch(Guid id, Guid tenantId, string externalId, Guid productId, Guid originFacilityId,
        Guid destinationFacilityId) : base(id)
    {
        if (tenantId == Guid.Empty)
            throw new BusinessRuleException("TenantId is mandatory");

        if (string.IsNullOrWhiteSpace(externalId))
            throw new BusinessRuleException("ExternalId is mandatory");

        if (productId == Guid.Empty)
            throw new BusinessRuleException("ProductId is mandatory");

        if (originFacilityId == Guid.Empty)
            throw new BusinessRuleException("OriginFacilityId is mandatory");

        if (destinationFacilityId == Guid.Empty)
            throw new BusinessRuleException("DestinationFacilityId is mandatory");

        TenantId = tenantId;
        ExternalId = externalId;
        ProductId = productId;
        OriginFacilityId = originFacilityId;
        DestinationFacilityId = destinationFacilityId;
    }

    public static Dispatch Create(Guid tenantId, string externalId, Guid productId, Guid originFacilityId,
        Guid destinationFacilityId)
    {
        var id = Guid.CreateVersion7();
        var dispatch = new Dispatch(id, tenantId, externalId, productId, originFacilityId, destinationFacilityId);
        dispatch.ChangeStatus(DispatchStatus.Created);
        return dispatch;
    }

    public void ChangeStatus(DispatchStatus newStatus, string? reason = null)
    {
        CurrentStatus = newStatus;
        _statusHistory.Add(new DispatchStatusTransition(newStatus, DateTime.UtcNow, reason));
    }

    public void AssignVehicle(Guid vehicleId)
    {
        if (vehicleId == Guid.Empty)
            throw new BusinessRuleException("VehicleId is mandatory");

        VehicleId = vehicleId;
    }

    public void AssignDriver(Guid driverId)
    {
        if (driverId == Guid.Empty)
            throw new BusinessRuleException("DriverId is mandatory");

        DriverId = driverId;
    }
}