using System.Security.Cryptography;
using LogiPulse.Domain.Base;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Tenants;

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
    
    public Guid DestinyFacilityId { get; private set; }
    public virtual Facility DestinyFacility { get; private set; }

    protected Dispatch()
    {
    }

    private Dispatch(Guid id, Guid tenantId, string externalId, Product product, Facility originFacility, Facility destinyFacility) : base(id)
    {
        TenantId = tenantId;
        ExternalId = externalId;
        ProductId = product.Id;
        Product = product;
        OriginFacilityId = originFacility.Id;
        OriginFacility = originFacility;
        DestinyFacilityId = destinyFacility.Id;
        DestinyFacility = destinyFacility;
    }

    public static Dispatch Create(string externalId, Product product, Facility originFacility, Facility destinyFacility)
    {
        var id = Guid.CreateVersion7();
        var dispatch = new Dispatch(id, product.TenantId, externalId, product, originFacility, destinyFacility);
        dispatch.ChangeStatus(DispatchStatus.Created);
        return dispatch;
    }

    public void ChangeStatus(DispatchStatus newStatus, string? reason = null)
    {
        CurrentStatus = newStatus;
        _statusHistory.Add(new DispatchStatusTransition(newStatus, DateTime.UtcNow, reason));
    }
}