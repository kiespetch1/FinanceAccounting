using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

public interface IUsersService
{
    Task<User> Get(int id);
    List<User> GetList();
    Task Delete(int id);
    Task Update(int id, UserUpdateData userUpdateData);
}