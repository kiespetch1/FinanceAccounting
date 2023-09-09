using ApplicationCore.Models;
using ApplicationCore.Services;
using Entities.Entities;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using static ApplicationCore.Utils.PasswordHashing;

namespace FinanceAccounting.Tests;

public class AuthServiceTest
{
    [Fact]
    public async void Registration_Successful_When_ValidDataProvided()
    {
        var ctx = TestDataHelper.CreateMockDb().Object;
        var validator = new Mock<IValidator<RegistrationData>>();
        var service = new AuthService(ctx, validator.Object);
        var user = new RegistrationData
        {
            Login = "login4",
            Name = "Ivan",
            MiddleName = "Ivanovich",
            LastName = "Ivanov",
            BirthDate = new DateTime(2023, 01, 01),
            Password = "String1!",
            ConfirmPassword = "String1!",
            Email = "email4@mail.com"
        };

        await service.Register(user);


        var isUserExists = ctx.Users.SingleOrDefault(x => x.Login == user.Login && x.Name == user.Name &&
                                                          x.MiddleName == user.MiddleName &&
                                                          x.LastName == user.LastName &&
                                                          x.BirthDate == user.BirthDate &&
                                                          VerifyHashedPassword(x.Password, user.Password)) != null;
        Assert.True(isUserExists);
    }

    [Fact]
    public async void Authorization_Successful_When_ValidDataProvided()
    {
        var ctx = TestDataHelper.CreateMockDb().Object;
        var validator = new Mock<IValidator<RegistrationData>>();
        var service = new AuthService(ctx, validator.Object);

        var authUser = new AuthData
        {
            Email = "email@mail.com",
            Password = "String1!"
        };

        service.Login(authUser);

        var isUserAuthorized = true;
        Assert.True(isUserAuthorized);
    }
}