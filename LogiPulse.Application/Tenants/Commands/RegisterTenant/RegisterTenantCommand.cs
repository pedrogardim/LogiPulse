using MediatR;

namespace LogiPulse.Application.Tenants.Commands.RegisterTenant;

public record RegisterTenantCommand : IRequest<Guid>
{
    public string TaxCode;
    public string DisplayName;
    public Guid AdminUserEntraId;
    public string AdminUserEmail;
    public string AdminUserName;
}