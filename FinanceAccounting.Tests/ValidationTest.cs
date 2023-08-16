using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using PublicApi.Validators;
using FluentValidation.TestHelper;

namespace FinanceAccounting.Tests;

public class ValidationTest
{
    [Fact]
    public void WhenAllRequirementsAreMet_ShouldNotHaveValidationError()
    {
        var validator = new AuthValidator();
        var model = new RegistrationData
        {
            Password = "String1!",
            ConfirmPassword = "String1!"
        };

        var result = validator.TestValidate(model);


        result.ShouldNotHaveValidationErrorFor(x => x.Password);
        result.ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword);
    }
    
    [Fact]
    public void WhenConfirmPasswordIsNotEqualPassword_ShouldHaveValidationError()
    {
        var validator = new AuthValidator();
        var model = new RegistrationData
        {
            Password = "String1!",
            ConfirmPassword = "tring1!"
        };

        var result = validator.TestValidate(model);


        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
    }
    
    [Theory]
    [InlineData("string1!", "string1!")]
    [InlineData("String!", "String!")]
    [InlineData("String1", "String1")]
    [InlineData("Str1!", "Str1!")]
    public void WhenSomeRequirementsAreNotMet_ShouldHaveValidationError(string password, string confirmPassword)
    {
        var validator = new AuthValidator();
        var model = new RegistrationData
        {
            Password = password,
            ConfirmPassword = confirmPassword
        };

        var result = validator.TestValidate(model);
        
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}