namespace LogiPulse.Application.Tenants.DTOs;

public record CreateTenantRequest(
    string DisplayName,
    string TaxCode
);