using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

public interface IExpenseService
{
    Task<Expense> Create(int userId, IncomeCreateData incomeCreateData);
    
    Task<List<Expense>> GetList(int userId, IncomeSearchContext incomeSearchContext);
    
    Task<Expense> Get(int id, int userId);

    Task Update(int userId, IncomeUpdateData incomeUpdateData);
    
    Task Delete(int id, int userId);
}