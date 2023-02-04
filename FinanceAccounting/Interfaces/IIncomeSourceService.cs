namespace FinanceAccounting.Interfaces;

public interface IIncomeSourceService
{
    Task Create(string incomeName, int userId);
    
    List<IncomeSource> GetList(int userId);

    Task<IncomeSource> Get(int id, int userId);
    
    Task Delete(int id, int userId);

    Task Update(int id, string newName, int userId);
}