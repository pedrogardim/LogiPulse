using FluentAssertions;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Tests.Entities.Dispatches;

public class DispatchTests
{
    private const string ExternalId = "D-001";

    [Fact]
    public void Create_ReturnsDispatch()
    {
        var tenantId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var originFacilityId = Guid.NewGuid();
        var destinationFacilityId = Guid.NewGuid();

        var dispatch = Dispatch.Create(tenantId, ExternalId, productId, originFacilityId, destinationFacilityId);

        dispatch.Should().NotBeNull();

        dispatch.TenantId.Should().Be(tenantId);
        dispatch.ExternalId.Should().Be(ExternalId);
        dispatch.ProductId.Should().Be(productId);
        dispatch.OriginFacilityId.Should().Be(originFacilityId);
        dispatch.DestinationFacilityId.Should().Be(destinationFacilityId);
    }
    
    [Fact]
    public void Create_SetStatusAsCreated()
    {
        var dispatch = Dispatch.Create(Guid.NewGuid(), ExternalId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        
        dispatch.CurrentStatus.Should().Be(DispatchStatus.Created);
        dispatch.StatusHistory.Count.Should().Be(1);

        var status = dispatch.StatusHistory.First();
        status.Status.Should().Be(DispatchStatus.Created);
        status.Reason.Should().BeNull();
    }

    [Fact]
    public void Create_InvalidTenantId_ThrowsException()
    {
        Action act = () => Dispatch.Create(Guid.Empty, ExternalId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("TenantId is mandatory");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidExternalId_ThrowsException(string? externalId)
    {
        Action act = () => Dispatch.Create(Guid.NewGuid(), externalId!, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("ExternalId is mandatory");
    }

    [Fact]
    public void Create_InvalidProductId_ThrowsException()
    {
        Action act = () => Dispatch.Create(Guid.NewGuid(), ExternalId, Guid.Empty, Guid.NewGuid(), Guid.NewGuid());

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("ProductId is mandatory");
    }

    [Fact]
    public void Create_InvalidOriginFacilityId_ThrowsException()
    {
        Action act = () => Dispatch.Create(Guid.NewGuid(), ExternalId, Guid.NewGuid(), Guid.Empty, Guid.NewGuid());

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("OriginFacilityId is mandatory");
    }

    [Fact]
    public void Create_InvalidDestinationFacilityId_ThrowsException()
    {
        Action act = () => Dispatch.Create(Guid.NewGuid(), ExternalId, Guid.NewGuid(), Guid.NewGuid(), Guid.Empty);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("DestinationFacilityId is mandatory");
    }
    
    [Fact]
    public void ChangeStatus_AddsToStatusHistory()
    {
        var dispatch = Dispatch.Create(Guid.NewGuid(), ExternalId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        
        dispatch.CurrentStatus.Should().Be(DispatchStatus.Created);
        dispatch.StatusHistory.Should().ContainSingle();
        
        var time = DateTime.UtcNow; 
        dispatch.ChangeStatus(DispatchStatus.InTransit, "Sent");
        dispatch.StatusHistory.Should().HaveCount(2);
        
        var lastStatus = dispatch.StatusHistory.Last();
        lastStatus.Status.Should().Be(DispatchStatus.InTransit);
        lastStatus.Reason.Should().Be("Sent");
        lastStatus.OcurredAtUtc.Should().BeCloseTo(time, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void AssignVehicle_ShouldAssign()
    {
        var vehicleId = Guid.NewGuid();
        var dispatch = Dispatch.Create(Guid.NewGuid(), ExternalId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            
        dispatch.AssignVehicle(vehicleId);
        dispatch.VehicleId.Should().Be(vehicleId);

    }
    
    [Fact]
    public void AssignVehicle_InvalidVehicleId_ThrowsException()
    {
        var dispatch = Dispatch.Create(Guid.NewGuid(), ExternalId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            
        Action act = () => dispatch.AssignVehicle(Guid.Empty);
        
        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("VehicleId is mandatory");

    }
    
    [Fact]
    public void AssignDriver_ShouldAssign()
    {
        var driverId = Guid.NewGuid();
        var dispatch = Dispatch.Create(Guid.NewGuid(), ExternalId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            
        dispatch.AssignDriver(driverId);
        dispatch.DriverId.Should().Be(driverId);
    }
    
    [Fact]
    public void AssignDriver_InvalidDriverId_ThrowsException()
    {
        var dispatch = Dispatch.Create(Guid.NewGuid(), ExternalId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            
        Action act = () => dispatch.AssignDriver(Guid.Empty);
        
        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("DriverId is mandatory");

    }
}