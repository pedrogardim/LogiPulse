namespace LogiPulse.Domain.Base;

public abstract class Entity : IHasTimestamps
{
    public Guid Id { get; private init; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    protected Entity()
    {
    }

    protected Entity(Guid id) => Id = id;

    public override bool Equals(object? obj) =>
        obj is Entity entity && Equals(entity);

    public bool Equals(Entity? other) =>
        other is not null && other.Id == Id;

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right) =>
        left is not null && left.Equals(right);

    public static bool operator !=(Entity? left, Entity? right) =>
        !(left == right);
}