namespace LogiPulse.Domain.Entities.Products;
public record ProductRuleMetric(string Code)
{
    public static readonly ProductRuleMetric Temperature = new("TEMP");
    public static readonly ProductRuleMetric Humidity = new("HUM");
    public static readonly ProductRuleMetric TiltAngle = new("TILT");
}