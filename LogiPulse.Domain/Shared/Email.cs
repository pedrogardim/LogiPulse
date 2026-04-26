using System.Text.RegularExpressions;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Shared;

public partial record Email 
{
    public string Value { get; init; }

    private Email(string value) => Value = value;
    
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new BusinessRuleException("Email cannot be empty");

        if (!EmailRegex().IsMatch(email))
            throw new BusinessRuleException("Invalid email format");

        return new Email(email.ToLowerInvariant());
    }
}