using FinanceAccounting.Entities;
using FinanceAccounting.Models;
using FinanceAccounting.SearchContexts;

namespace FinanceAccounting.Interfaces;

public interface IExpenseService
{
    Task<Expense> Create(int userId, ExpenseCreateData expenseCreateData);
    
    Task<List<Expense>> GetList(int userId, ExpenseSearchContext expenseSearchContext);
    
    Task<Expense> Get(int id, int userId);

    Task Update(int userId, ExpenseUpdateData expenseUpdateData);
    
    Task Delete(int id, int userId);
}