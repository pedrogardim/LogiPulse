using FluentAssertions;
using LogiPulse.Domain.Base;

namespace LogiPulse.Domain.Tests.Base;

public class EntityTests
{
    class TestEntity : Entity
    {
        public TestEntity(Guid id) : base(id)
        {
        }
    }

    [Fact]
    public void Entity_CreateWithId()
    {
        var id = Guid.NewGuid();
        var entity = new TestEntity(id);
        entity.Should().NotBeNull();
        entity.Id.Should().Be(id);
    }
    
    [Fact]
    public void ShouldCompareById()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);
        
        var entity3 = new TestEntity(Guid.NewGuid());
        entity1.Should().Be(entity2);
        
        entity1.Should().NotBe(entity3);
        entity2.Should().NotBe(entity3);
        
        Assert.True(entity1 == entity2);
        Assert.True(entity1 != entity3);
    }
    
    [Fact]
    public void ShouldGetHashCodeById()
    {
        var id = Guid.NewGuid();
        var entity = new TestEntity(id);
       
        id.GetHashCode().Should().Be(entity.GetHashCode());
    }
}