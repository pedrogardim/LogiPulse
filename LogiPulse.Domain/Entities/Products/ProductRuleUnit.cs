namespace LogiPulse.Domain.Entities.Products;

public record ProductRuleUnit(string Symbol)
{
    public static readonly ProductRuleUnit Celsius = new("C");
    public static readonly ProductRuleUnit Percentage = new("%");
    public static readonly ProductRuleUnit Lux = new("LUX");
}