using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

public interface IIncomeService
{
    Task<Income> Create(string incomeName, int userId, float amount, int categoryId);
    
    Task<List<Income>> GetList(int userId, DateTime from, DateTime to);
    
    Task<Income> Get(int id, int userId);

    Task Update(int id, int userId, CategoryUpdateData categoryUpdateData);
    
    Task Delete(int id, int userId);
}