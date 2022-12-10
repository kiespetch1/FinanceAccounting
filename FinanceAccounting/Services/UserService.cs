using FinanceAccounting.Exceptions;
using FinanceAccounting.Models;
using static FinanceAccounting.PasswordHashing;

namespace FinanceAccounting.Services;

public static class UserService
{
    public interface IGetService
    {
        Task<User> Get(int id);
    }
    public class GetService : IGetService
    {
        public async Task<User> Get(int id)
        {
            await using var ctx = new ApplicationContext();

            var user = ctx.Users.SingleOrDefault(x => x.Id == id);
            if (user == null)
                throw new UserNotFoundException();
            return user;
        }
    }
    
    public interface IGetListService
    {
        Task<List<User>> GetList();
    }

    public class GetListService : IGetListService
    {
        public async Task<List<User>> GetList()
        {
            await using var ctx = new ApplicationContext();

            var allUsers = ctx.Users.ToList();
            return allUsers;
        }
    }
    
    public interface IDeleteService
    {
        void Delete(int id);
    }

    public class DeleteService : IDeleteService
    {
        public async void Delete(int id)
        {
            await using var ctx = new ApplicationContext();

            var user = ctx.Users.SingleOrDefault(x => x.Id == id);
            if (user == null)
                throw new UserNotFoundException();
            ctx.Users.Remove(user);
            await ctx.SaveChangesAsync();
        }
    }
    
    public interface IUpdateService 
    {
        void Update(UserUpdateData userUpdateData, int id);
    }

    public class UpdateService : IUpdateService
    {
        public async void Update(UserUpdateData userUpdateData, int id)
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
}