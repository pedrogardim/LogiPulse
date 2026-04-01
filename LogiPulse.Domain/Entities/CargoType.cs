namespace LogiPulse.Domain.Entities;

public class CargoType
{
    public Guid Id { get; set; }
    public ICollection<Order> Orders { get; }
}