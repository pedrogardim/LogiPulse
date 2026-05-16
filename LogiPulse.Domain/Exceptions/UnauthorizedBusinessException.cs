namespace LogiPulse.Domain.Exceptions;

public class UnauthorizedBusinessException(string message) : Exception(message);