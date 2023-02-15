using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

public interface IExpenseService
{
    Task<Expense> Create(string expenseName, int userId, float amount, int categoryId);

    Task<List<Expense>> GetList(int userId, DateTime from, DateTime to);

    Task<Expense> Get(int id, int userId);

    Task Update(int id, int userId, CategoryUpdateData expenseUpdateData);

    Task Delete(int id, int userId);
}