namespace LogiPulse.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }

    public Guid CargoTypeId { get; set; }
    public virtual CargoType CargoType { get; set; }
}