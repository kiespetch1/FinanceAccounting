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
    
    [Fact]
    public void WhenThereIsNoCharactersInUpperCase_ShouldHaveValidationError()
    {
        var validator = new AuthValidator();
        var model = new RegistrationData
        {
            Password = "string1!",
            ConfirmPassword = "string1!"
        };

        var result = validator.TestValidate(model);


        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    [Fact]
    public void WhenThereIsNoNumbers_ShouldHaveValidationError()
    {
        var validator = new AuthValidator();
        var model = new RegistrationData
        {
            Password = "String!",
            ConfirmPassword = "String!"
        };

        var result = validator.TestValidate(model);


        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    [Fact]
    public void WhenThereIsNoSpecialCharacters_ShouldHaveValidationError()
    {
        var validator = new AuthValidator();
        var model = new RegistrationData
        {
            Password = "String1",
            ConfirmPassword = "String1"
        };

        var result = validator.TestValidate(model);


        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    [Fact]
    public void WhenThereIsLessThanSixCharacters_ShouldHaveValidationError()
    {
        var validator = new AuthValidator();
        var model = new RegistrationData
        {
            Password = "Str1!",
            ConfirmPassword = "Str1!"
        };

        var result = validator.TestValidate(model);


        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
}