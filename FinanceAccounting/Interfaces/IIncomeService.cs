using FinanceAccounting.Entities;
using FinanceAccounting.Models;
using FinanceAccounting.SearchContexts;

namespace FinanceAccounting.Interfaces;

public interface IIncomeService
{
    Task<Income> Create(int userId, IncomeCreateData incomeCreateData);
    
    Task<List<Income>> GetList(int userId, IncomeSearchContext incomeSearchContext);
    
    Task<Income> Get(int id, int userId);

    Task Update(int userId, IncomeUpdateData incomeUpdateData);
    
    Task Delete(int id, int userId);
}