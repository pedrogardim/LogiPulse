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
    
    public Guid FacilityId { get; private set; }
    public virtual Facility Facility { get; private set; }

    protected Dispatch()
    {
    }

    private Dispatch(Guid id, Guid tenantId, string externalId, Product product, Facility facility) : base(id)
    {
        TenantId = tenantId;
        ExternalId = externalId;
        ProductId = product.Id;
        Product = product;
        FacilityId = facility.Id;
        Facility = facility;
    }

    public static Dispatch Create(string externalId, Product product, Facility facility)
    {
        var id = Guid.CreateVersion7();
        var dispatch = new Dispatch(id, product.TenantId, externalId, product, facility);
        dispatch.ChangeStatus(DispatchStatus.Created);
        return dispatch;
    }

    public void ChangeStatus(DispatchStatus newStatus, string? reason = null)
    {
        CurrentStatus = newStatus;
        _statusHistory.Add(new DispatchStatusTransition(newStatus, DateTime.UtcNow, reason));
    }
}