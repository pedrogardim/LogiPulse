namespace LogiPulse.Application.Tenants.DTOs;

public record RegisterTenantRequest(
    string DisplayName,
    string TaxCode
);