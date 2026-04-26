using FluentAssertions;
using LogiPulse.Domain.Entities.Dispatches;

namespace LogiPulse.Domain.Tests.Entities.Dispatches;

public class DispatchStatusTransitionTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("Some reason")]
    public void Create_ReturnDispatchStatusTransition(string? reason)
    {
        var now = DateTime.UtcNow;
        var transition = new DispatchStatusTransition(DispatchStatus.Created, now, reason);
        transition.Should().NotBeNull();

        transition.Status.Should().Be(DispatchStatus.Created);
        transition.OccurredAtUtc.Should().Be(now);
        transition.Reason.Should().Be(reason ?? null);
    }
    
    [Fact]
    public void ShouldCompareByValue()
    {
        var now = DateTime.UtcNow;
        var transition1 = new DispatchStatusTransition(DispatchStatus.Created, now, "System creation");
        var transition2 = new DispatchStatusTransition(DispatchStatus.Created, now, "System creation");
        transition1.Should().Be(transition2); 
    }
}