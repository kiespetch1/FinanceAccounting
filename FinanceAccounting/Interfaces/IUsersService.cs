using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

public interface IUsersService
{
    Task<User> Get(int id);
    Task<List<User>> GetList();
    void Delete(int id);
    Task Update(int id, UserUpdateData userUpdateData);
}