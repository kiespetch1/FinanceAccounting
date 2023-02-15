using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

public interface IIncomeSourceService
{
    Task<IncomeSource> Create(string incomeName, int userId);

    Task<List<IncomeSource>> GetList(int userId);

    Task<IncomeSource> Get(int id, int userId);

    Task Update(int id, string newName, int userId);

    Task Delete(int id, int userId);
}