using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

public interface IUsersService
{
    Task<List<User>> GetList();
    
    Task<User> Get(int id);
    
    Task Update(int id, UserUpdateData userUpdateData);
    
    Task Delete(int id);
    
}