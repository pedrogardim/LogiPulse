namespace LogiPulse.Domain.Shared.Transport;

public record TransportUnit(string Symbol)
{
    public static readonly TransportUnit Celsius = new("C");
    public static readonly TransportUnit Percentage = new("%");
    public static readonly TransportUnit Lux = new("LUX");
}