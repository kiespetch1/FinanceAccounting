using ApplicationCore.Utils;
using Entities.Entities;
using Infrastructure;
using Moq;
using Moq.EntityFrameworkCore;

namespace FinanceAccounting.Tests;

public class TestDataHelper
{
    private static List<User> GetMockUsers()
    {
        return new List<User>
        {
            new()
            {
                Login = "login",
                Name = "Ivan",
                MiddleName = "Ivanovich",
                LastName = "Ivanov",
                BirthDate = new DateTime(2023, 01, 01),
                Password = PasswordHashing.HashPassword("String1!"),
                Email = "email@mail.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new()
            {
                Login = "login2",
                Name = "Ivan2",
                MiddleName = "Ivanovich2",
                LastName = "Ivanov2",
                BirthDate = new DateTime(2023, 01, 02),
                Password = PasswordHashing.HashPassword("String2!"),
                Email = "email2@mail.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            }
        };
    }

    internal static Mock<IDatabaseContext> CreateMockDb()
    {
        var dbContextMock = new Mock<IDatabaseContext>();
        dbContextMock.Setup(x => x.Users).ReturnsDbSet(GetMockUsers());

        return dbContextMock;
    }

}