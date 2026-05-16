namespace LogiPulse.Application.Tenants.Commands.RegisterTenant;

public record RegisterTenantRequest(
    string DisplayName,
    string TaxCode
);