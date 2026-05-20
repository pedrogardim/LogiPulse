using FluentAssertions;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using NetTopologySuite.Geometries;

namespace LogiPulse.Domain.Tests.Entities.Facilities;

public class FacilityTests
{
    private const string ExternalId = "CD-GRU-01";
    private const string Name = "CD Pfizer Guarulhos";
    private const string Code = "CD-GRU";

    private readonly Guid _tenantId = Guid.NewGuid();

    private readonly Address _address =
        new("Rod. Hélio Smidt", "s/n", "Guarulhos", "SP", "07190-100", "Brazil", "Aeroporto");

    private readonly Point _point = new(-46.789, -23.562) { SRID = 4326 };

    [Fact]
    public void Create_ReturnsFacility()
    {
        var facility = Facility.Create(_tenantId, ExternalId, Name, Code, FacilityType.ProductionPlant, _address,
            _point);
        facility.Should().NotBeNull();

        facility.TenantId.Should().Be(_tenantId);
        facility.ExternalId.Should().Be(ExternalId);
        facility.Name.Should().Be(Name);
        facility.Code.Should().Be(Code);
        facility.Type.Should().Be(FacilityType.ProductionPlant);
        facility.Address.Should().Be(_address);
        facility.Location.Should().Be(_point);
    }

    [Fact]
    public void Create_InvalidTenantId_ThrowsException()
    {
        Action act = () =>
            Facility.Create(Guid.Empty, ExternalId, Name, Code, FacilityType.ProductionPlant, _address, _point);

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
            Facility.Create(_tenantId, externalId!, Name, Code, FacilityType.ProductionPlant, _address, _point);

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
            Facility.Create(_tenantId, ExternalId, name!, Code, FacilityType.ProductionPlant, _address, _point);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Name is mandatory");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Create_InvalidCode_ThrowsException(string? code)
    {
        Action act = () =>
            Facility.Create(_tenantId, ExternalId, Name, code!, FacilityType.ProductionPlant, _address, _point);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Code is mandatory");
    }

    [Fact]
    public void Update_UpdatesFacility()
    {
        var facility = Facility.Create(
            _tenantId,
            ExternalId,
            "_",
            "_",
            FacilityType.DeliveryPoint,
            new Address("_", "_", "_", "_", "_", "_", "_"),
            new Point(0, 0));

        facility.Update(Name, Code, FacilityType.ProductionPlant, _point.Y, _point.X, _address);

        facility.TenantId.Should().Be(_tenantId);
        facility.ExternalId.Should().Be(ExternalId);
        facility.Name.Should().Be(Name);
        facility.Code.Should().Be(Code);
        facility.Type.Should().Be(FacilityType.ProductionPlant);
        facility.Address.Should().Be(_address);
        facility.Location.Should().Be(_point);
    }
}