using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Services;
using FluentValidation;
using Moq;

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
            Login = "login",
            Name = "Ivan",
            MiddleName = "Ivanovich",
            LastName = "Ivanov",
            BirthDate = new DateTime(2023, 01, 01),
            Password = "String1!",
            Email = "email@mail.com",
        };

        await Assert.ThrowsAsync<ExistingLoginException>(async () =>
        {
            await service.Register(user);
        });
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

        var result = service.Login(authUser);
        Assert.NotNull(result);
    }
}