using FluentAssertions;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Tests.Entities.Vehicles;

public class VehicleTests
{
    private const string ExternalId = "V-001";
    private const string Name = "Peugeot 123";
    private const string LicensePlate = "ABC-1234";

    private readonly Guid _tenantId = Guid.NewGuid();

    [Fact]
    public void Create_ReturnsVehicle()
    {
        var vehicle = Vehicle.Create(_tenantId, ExternalId, Name, LicensePlate, VehicleType.Van);
        vehicle.Should().NotBeNull();

        vehicle.TenantId.Should().Be(_tenantId);
        vehicle.ExternalId.Should().Be(ExternalId);
        vehicle.Name.Should().Be(Name);
        vehicle.LicensePlate.Should().Be(LicensePlate);
        vehicle.Type.Should().Be(VehicleType.Van);

        vehicle.HomeFacilityId.Should().Be(null);
        vehicle.IsActive.Should().Be(false);
    }

    [Fact]
    public void Create_InvalidTenantId_ThrowsException()
    {
        Action act = () =>
            Vehicle.Create(Guid.Empty, ExternalId, Name, LicensePlate, VehicleType.Van);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("TenantId is mandatory");
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidExternalId_ThrowsException(string? externalId)
    {
        Action act = () =>
            Vehicle.Create(_tenantId, externalId!, Name, LicensePlate, VehicleType.Van);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("ExternalId is mandatory");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidName_ThrowsException(string? name)
    {
        Action act = () =>
            Vehicle.Create(_tenantId, ExternalId, name!, LicensePlate, VehicleType.Van);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Name is mandatory");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidLicensePlate_ThrowsException(string? licensePlate)
    {
        Action act = () =>
            Vehicle.Create(_tenantId, ExternalId, Name, licensePlate!, VehicleType.Van);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("LicensePlate is mandatory");
    }
    
    [Fact]
    public void Activate_SetActiveFlagTrue()
    {
        var vehicle = Vehicle.Create(_tenantId, ExternalId, Name, LicensePlate, VehicleType.Van);
        vehicle.IsActive.Should().Be(false);
        vehicle.Activate();
        vehicle.IsActive.Should().Be(true);
    }
    
    [Fact]
    public void Deactivate_SetActiveFlagFalse()
    {
        var vehicle = Vehicle.Create(_tenantId, ExternalId, Name, LicensePlate, VehicleType.Van);
        vehicle.Activate();
        vehicle.Deactivate();
        vehicle.IsActive.Should().Be(false);
    }
    
    [Fact]
    public void AssignToFacility_SetHomeFacility()
    {
        var facilityId = Guid.NewGuid();
        var vehicle = Vehicle.Create(_tenantId, ExternalId, Name, LicensePlate, VehicleType.Van);
        vehicle.HomeFacilityId.Should().BeNull();
        vehicle.AssignToFacility(facilityId);
        vehicle.HomeFacilityId.Should().Be(facilityId);
    }
    
    [Fact]
    public void UnassignFacility_ClearHomeFacility()
    {
        var facilityId = Guid.NewGuid();
        var vehicle = Vehicle.Create(_tenantId, ExternalId, Name, LicensePlate, VehicleType.Van);
        vehicle.AssignToFacility(facilityId);
        vehicle.UnassignFacility();
        vehicle.HomeFacilityId.Should().BeNull();
    }
    
    [Fact]
    public void SetCapability_AddCapability()
    {
        var vehicle = Vehicle.Create(_tenantId, ExternalId, Name, LicensePlate, VehicleType.Van);
        
        vehicle.SetCapability( "TEMP", "CELSIUS",-5m, 10);
        vehicle.Capabilities.Count.Should().Be(1);
        var newRequirement = vehicle.Capabilities.First();

        // Value Object Metric
        newRequirement.MetricCode.Should().Be("TEMP");
        newRequirement.Metric.Code.Should().Be("TEMP");
        
        // Value Object Unit
        newRequirement.Unit.Should().Be("CELSIUS");
        newRequirement.RuleUnit.Symbol.Should().Be("CELSIUS");
        
        newRequirement.Min.Should().Be(-5m);
        newRequirement.Max.Should().Be(10);
    }
    
    [Fact]
    public void SetCapability_OverwriteRequirement()
    {
        var vehicle = Vehicle.Create(_tenantId, ExternalId, Name, LicensePlate, VehicleType.Van);

        vehicle.SetCapability( "TEMP", "FAHRENHEIT",-23m, 50);
        vehicle.SetCapability( "TEMP", "CELSIUS",-5m, 10m);
        
        vehicle.Capabilities.Count.Should().Be(1);
        var newRequirement = vehicle.Capabilities.First();

        newRequirement.MetricCode.Should().Be("TEMP");
        newRequirement.Unit.Should().Be("CELSIUS");
        newRequirement.Min.Should().Be(-5m);
        newRequirement.Max.Should().Be(10m);
    }
    
    [Fact]
    public void ClearRequirements_ShouldEmptyTheRequirementsList()
    {
        var vehicle = Vehicle.Create(_tenantId, ExternalId, Name, LicensePlate, VehicleType.Van);
        vehicle.SetCapability("TEMP", "CELSIUS", -5m, 10m);
        vehicle.SetCapability("HUMIDITY", "PERCENT", 40m, 60m);
        vehicle.Capabilities.Count.Should().Be(2);
        
        vehicle.ClearCapabilities();

        vehicle.Capabilities.Should().BeEmpty();
    }
}