using System.Text.RegularExpressions;
using ApplicationCore.Models;
using FluentValidation;

namespace PublicApi;

public class AuthValidator : AbstractValidator<RegistrationData>
{
    public AuthValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .Must(IsPasswordValid)
            .WithMessage("The password cannot be less than 6 characters, the password must contain at least one numeric" +
                         " and one non-alphanumeric character, and at least one alphabetic character must be in uppercase.");
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("This field should be identical to password field.");
    }
    private static bool IsPasswordValid(string password)
    {
        var regex = new Regex(@"[!""#$%&'()*+,-./:;<=>?@[\\\]^_`{|}~]");
        
        return password.Any(char.IsUpper) &&
               password.Any(char.IsDigit) &&
               regex.IsMatch(password);
    }
}
