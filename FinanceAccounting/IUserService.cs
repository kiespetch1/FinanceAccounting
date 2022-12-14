using FinanceAccounting.Models;

namespace FinanceAccounting;

public interface IUserService
{
    Task<User> Get(int id);
    Task<List<User>> GetList();
    void Delete(int id);
    void Update(int id, UserUpdateData userUpdateData);
}