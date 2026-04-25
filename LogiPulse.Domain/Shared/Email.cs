using System.Text.RegularExpressions;
using LogiPulse.Domain.Base;
using LogiPulse.Domain.Exceptions;

namespace LogiPulse.Domain.Shared;

public class Email : ValueObject
{
    public string Value { get; }

    private Email(string email) => Value = email;

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new BusinessRuleException("Email cannot be empty");

        var valid = Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        if (!valid)
            throw new BusinessRuleException("Invalid email format");

        return new Email(email);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}