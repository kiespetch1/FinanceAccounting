using FinanceAccounting.Exceptions;
using FinanceAccounting.Models;
using static FinanceAccounting.PasswordHashing;

namespace FinanceAccounting.Services;

public class UserService : IUserService
{
    public async Task<User> Get(int id)
    {
        await using var ctx = new ApplicationContext();

        var user = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
            throw new UserNotFoundException();
        return user;
    }
    
    public async Task<List<User>> GetList()
    {
        await using var ctx = new ApplicationContext();

        var allUsers = ctx.Users.ToList();
        return allUsers;
    }

    public async void Delete(int id)
    {
        await using var ctx = new ApplicationContext();

        var user = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
            throw new UserNotFoundException();
        ctx.Users.Remove(user);
        await ctx.SaveChangesAsync();
    }
    
    public async void Update(int id, UserUpdateData userUpdateData)
    {
        await using var ctx = new ApplicationContext();

        var user = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
            throw new UserNotFoundException();

        if (ctx.Users.SingleOrDefault(x => x.Email == userUpdateData.Email && x.Id != id) != null)
            throw new ExistingLoginException();
        user.Email = userUpdateData.Email;
        user.Password = HashPassword(userUpdateData.Password);
        if (ctx.Users.SingleOrDefault(x => x.Login == userUpdateData.Login && x.Id != id) != null)
            throw new ExistingLoginException();
        user.Login = userUpdateData.Login;
        user.EditDate = DateTime.Today;
        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }
}