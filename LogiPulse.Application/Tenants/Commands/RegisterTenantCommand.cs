using LogiPulse.Domain.Entities.Tenants;
using MediatR;

namespace LogiPulse.Application.Tenants.Commands;

public record RegisterTenantCommand : IRequest<Guid>
{
    public string TaxCode;
    public string DisplayName;
    public Guid AdminUserEntraId;
    public string AdminUserEmail;
    public string AdminUserName;
}