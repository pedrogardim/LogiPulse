using LogiPulse.Domain.Base;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Entities.Drivers;

public class Driver : Entity
{
    public Guid TenantId { get; private set; }

    public Guid? UserId { get; private set; }
    public virtual User User { get; private set; }

    public string ExternalId { get; private set; }

    public string Name { get; private set; }
    public string Phone { get; private set; }

    public string LicenseNumber { get; private set; }
    public DateOnly LicenseExpiryDate { get; private set; }

    public bool IsActive { get; private set; }

    public virtual List<Dispatch> Dispatches { get; } = [];

    protected Driver()
    {
    }

    private Driver(Guid id, Guid tenantId, Guid? userId, string externalId, string name, string phone,
        string licenseNumber, DateOnly licenseExpiryDate, bool isActive) : base(id)
    {
        if (tenantId == Guid.Empty)
            throw new BusinessRuleException("TenantId is mandatory");
        if (userId != null && userId == Guid.Empty)
            throw new BusinessRuleException("UserId must be null or a non-empty Guid");

        if (string.IsNullOrWhiteSpace(externalId))
            throw new BusinessRuleException("ExternalId is mandatory");
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleException("Name is mandatory");
        if (string.IsNullOrWhiteSpace(phone))
            throw new BusinessRuleException("Phone is mandatory");
        if (string.IsNullOrWhiteSpace(licenseNumber))
            throw new BusinessRuleException("LicenseNumber is mandatory");
        if (licenseExpiryDate <= DateOnly.FromDateTime(DateTime.UtcNow.Date))
            throw new BusinessRuleException("Driver License is expired");

        TenantId = tenantId;
        UserId = userId;
        ExternalId = externalId;
        Name = name;
        Phone = phone;
        LicenseNumber = licenseNumber;
        LicenseExpiryDate = licenseExpiryDate;
        IsActive = isActive;
    }

    public static Driver Create(Guid tenantId, Guid? userId, string externalId, string name, string phone,
        string licenseNumber, DateOnly licenseExpiryDate)
    {
        var id = Guid.CreateVersion7();
        var driver = new Driver(id, tenantId, userId, externalId, name, phone, licenseNumber, licenseExpiryDate, true);
        return driver;
    }
}