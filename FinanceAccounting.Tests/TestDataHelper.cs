using ApplicationCore.Models;
using ApplicationCore.Utils;
using Entities.Entities;
using Infrastructure;
using Moq;
using Moq.EntityFrameworkCore;

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
        public bool Equals(TypeResponse<T> first, TypeResponse<T> second)
        {
            if (ReferenceEquals(first, second)) return true;

            if (first == null || second == null) return false;

            if (first.Total != second.Total) return false;
            
            if (first.Items.Count != second.Items.Count) return false;

            for (int i = 0; i < first.Items.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(first.Items[i], second.Items[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(TypeResponse<T> obj)
        {
            if (obj == null) return 0;
            
            return obj.Total.GetHashCode();
        }
    }

}