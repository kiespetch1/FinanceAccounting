using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

public interface IExpenseSourceService
{
    Task<ExpenseSource> Create(string expenseName, int userId);

    Task<List<ExpenseSource>> GetList(int userId);

    Task<ExpenseSource> Get(int id, int userId);

    Task Update(int id, string newName, int userId);

    Task Delete(int id, int userId);
}