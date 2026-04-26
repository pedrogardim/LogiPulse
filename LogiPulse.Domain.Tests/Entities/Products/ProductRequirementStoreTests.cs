using FluentAssertions;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Tests.Entities.Products;

public class ProductRequirementStoreTests
{
    private class TestStore : ProductRequirementStore
    {
        public TestStore(Guid id) : base(id)
        {
        }
    }

    [Fact]
    public void SetRequirement_AddRequirement()
    {
        var store = new TestStore(Guid.NewGuid());

        store.SetRequirement( "TEMP", "CELSIUS",-5m, 10);
        store.Requirements.Count.Should().Be(1);
        var newRequirement = store.Requirements.First();

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
    public void SetRequirement_OverwriteRequirement()
    {
        var store = new TestStore(Guid.NewGuid());

        store.SetRequirement( "TEMP", "FAHRENHEIT",-23m, 50);

        store.SetRequirement( "TEMP", "CELSIUS",-5m, 10);
        store.Requirements.Count.Should().Be(1);
        var newRequirement = store.Requirements.First();

        newRequirement.MetricCode.Should().Be("TEMP");
        newRequirement.Unit.Should().Be("CELSIUS");
        newRequirement.Min.Should().Be(-5m);
        newRequirement.Max.Should().Be(10);
    }
    
    [Fact]
    public void ClearRules_ShouldEmptyTheRequirementsList()
    {
        var store = new TestStore(Guid.NewGuid());
        store.SetRequirement("TEMP", "CELSIUS", -5m, 10m);
        store.SetRequirement("HUMIDITY", "PERCENT", 40m, 60m);

        store.Requirements.Count.Should().Be(2);
        
        store.ClearRules();

        store.Requirements.Should().BeEmpty();
    }
}