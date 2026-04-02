namespace LogiPulse.Domain.Entities.Dispatch;

public enum DispatchStatus
{
    Created = 1,
    Loading = 2,
    InTransit = 3,
    ArrivedAtDestination = 4,
    BackToFacility = 5,
    Completed = 6,
    
    Cancelled = 99
}