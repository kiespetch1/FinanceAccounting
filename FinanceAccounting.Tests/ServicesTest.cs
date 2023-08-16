using ApplicationCore.Models;
using ApplicationCore.Services;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using static ApplicationCore.Utils.PasswordHashing;

namespace FinanceAccounting.Tests;

public class ServicesTest
{
    public class AuthServiceTests
    {
        [Fact]
        public async void Registration_Successful_When_ValidDataProvided()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            
            await using (new ApplicationContext(options)){}
            var ctx = new ApplicationContext(options);
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
                ConfirmPassword = "String1!",
                Email = "email@mail.com"

            };

            await service.Register(user);
            

            var isUserExists = ctx.Users.SingleOrDefault(x => x.Login == user.Login && x.Name == user.Name && 
                                            x.MiddleName == user.MiddleName && x.LastName == user.LastName && 
                                            x.BirthDate == user.BirthDate && VerifyHashedPassword(x.Password, user.Password)) != null;
            Assert.True(isUserExists);

        }
    }
    
}