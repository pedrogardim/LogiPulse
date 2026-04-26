using FluentAssertions;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;

namespace LogiPulse.Domain.Tests.Shared;

public class EmailTests
{
    [Theory]
    [InlineData("demo@mail.com")]
    [InlineData("user@logipulse.io")]
    public void Create_ReturnEmail(string input)
    {
        var email = Email.Create(input);
        email.Should().NotBeNull();
        email.Value.Should().Be(input);
    }
    

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Create_EmptyEmail_ThrowsException(string? input)
    {
        Action act = () => Email.Create(input!);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Email cannot be empty");
    }
    
    [Theory]
    [InlineData("demomailcom")]
    [InlineData("demo@mailcom")]
    [InlineData("demomail.com")]
    public void Create_InvalidFormat_ThrowsException(string input)
    {
        Action act = () => Email.Create(input);

        act.Should().ThrowExactly<BusinessRuleException>()
            .WithMessage("Invalid email format");
    }
    
    [Fact]
    public void ShouldCompareByValue()
    {
        var email1 = Email.Create("admin@logipulse.io");
        var email2 = Email.Create("admin@logipulse.io");
        var email3 = Email.Create("user@logipulse.io");
        
        email1.Should().Be(email2);
        email1.Should().NotBe(email3);
        email2.Should().NotBe(email3);
    }
    
    [Fact]
    public void ShouldCompare_CaseInsensitive()
    {
        var email1 = Email.Create("admin@logipulse.io");
        var email2 = Email.Create("ADMIN@LOGIPULSE.IO");
        
        email1.Should().Be(email2);
    }
}