using System.Security.Cryptography;
using LogiPulse.Domain.Base;

namespace LogiPulse.Domain.Entities.Dispatches;

public class Dispatch : Entity
{
    public Guid TenantId { get; private set; }
    public string ExternalId { get; private set; }
    public DispatchStatus CurrentStatus { get; private set; }

    private readonly List<DispatchStatusTransition> _statusHistory = new();
    public IReadOnlyCollection<DispatchStatusTransition> StatusHistory => _statusHistory.AsReadOnly();

    protected Dispatch()
    {
    }

    private Dispatch(Guid id, Guid tenantId, string externalId) : base(id)
    {
        TenantId = tenantId;
        ExternalId = externalId;
    }

    public static Dispatch Create(string externalId)
    {
        var id = Guid.CreateVersion7();
        var tenantId = Guid.CreateVersion7(); // TODO: Tenant Logic

        var dispatch = new Dispatch(id, tenantId, externalId);
        dispatch.ChangeStatus(DispatchStatus.Created);

        return dispatch;
    }

    public void ChangeStatus(DispatchStatus newStatus, string? reason = null)
    {
        CurrentStatus = newStatus;
        _statusHistory.Add(new DispatchStatusTransition(newStatus, DateTime.UtcNow, reason));
    }
}