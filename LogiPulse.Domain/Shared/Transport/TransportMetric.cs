namespace LogiPulse.Domain.Shared.Transport;

public record TransportMetric(string Code)
{
    public static readonly TransportMetric Temperature = new("TEMP");
    public static readonly TransportMetric Humidity = new("HUM");
    public static readonly TransportMetric TiltAngle = new("TILT");
}