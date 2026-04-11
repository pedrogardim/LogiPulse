namespace LogiPulse.Domain.Base;

public interface IHasTimestamps
{
    DateTime CreatedAtUtc { set; get; }
    DateTime UpdatedAtUtc { set; get; }
}