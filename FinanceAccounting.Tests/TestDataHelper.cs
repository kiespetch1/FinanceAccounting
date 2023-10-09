using System.Reflection;
using ApplicationCore.Models;
using ApplicationCore.Utils;
using Entities.Entities;
using Infrastructure;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit.Sdk;

namespace FinanceAccounting.Tests;

public class TestDataHelper
{
    internal static Mock<IDatabaseContext> CreateMockDb()
    {
        var dbContextMock = new Mock<IDatabaseContext>();
        dbContextMock.Setup(x => x.Users).ReturnsDbSet(GetUsersMock());
        dbContextMock.Setup(x => x.Income).ReturnsDbSet(GetIncomeMock());
        dbContextMock.Setup(x => x.IncomeSources).ReturnsDbSet(GetIncomeSourcesMock());
        dbContextMock.Setup(x => x.Expense).ReturnsDbSet(GetExpenseMock());
        dbContextMock.Setup(x => x.ExpenseSources).ReturnsDbSet(GetExpenseSourcesMock());
        dbContextMock.Setup(x => x.ExpenseSources).ReturnsDbSet(GetExpenseSourcesMock());

        return dbContextMock;
    }

    private static List<Income> GetIncomeMock()
    {
        return new List<Income>
        {
            new()
            {
                Amount = 300.33m,
                CategoryId = 1,
                Id = 1,
                Name = "incomeName",
                User = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new()
            {
                Amount = 300.32m,
                CategoryId = 1,
                Id = 2,
                Name = "incomeName2",
                User = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };
    }

    internal static List<Expense> GetExpenseMock()
    {
        return new List<Expense>
        {
            new()
            {
                Amount = 300.32m,
                CategoryId = 1,
                Id = 1,
                Name = "expenseName",
                User = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new()
            {
                Amount = 300.33m,
                CategoryId = 1,
                Id = 2,
                Name = "expenseName2",
                User = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };
    }

    private static List<IncomeSource> GetIncomeSourcesMock()
    {
        return new List<IncomeSource>
        {
            new()
            {
                Id = 1,
                UserId = 1
            }
        };
    }

    private static List<ExpenseSource> GetExpenseSourcesMock()
    {
        return new List<ExpenseSource>
        {
            new()
            {
                Id = 1,
                UserId = 1
            }
        };
    }

    private static List<User> GetUsersMock()
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
            }
        };
    }

    public static bool DatesAreEqualIgnoringMilliseconds(DateTime date1, DateTime date2)
    {
        var timeDifference = date1 - date2;
        return Math.Abs(timeDifference.TotalSeconds) < 1;
    }

    public class TypeResponseComparer<T> : IEqualityComparer<TypeResponse<T>>
    {
        public bool Equals(TypeResponse<T> x, TypeResponse<T> y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            if (x.Total != y.Total)
                return false;

            if (x.Items.Count != y.Items.Count)
                return false;
            
            for (int i = 0; i < x.Items.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(x.Items[i], y.Items[i]))
                    return false;
            }

            return true;
        }

        public int GetHashCode(TypeResponse<T> obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var hash = 17;
            hash = hash * 23 + obj.Total.GetHashCode();

            foreach (var item in obj.Items)
            {
                hash = hash * 23 + EqualityComparer<T>.Default.GetHashCode(item);
            }

            return hash;
        }
    }

}