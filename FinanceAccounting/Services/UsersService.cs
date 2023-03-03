using FinanceAccounting.Entities;
using Microsoft.EntityFrameworkCore;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
using static FinanceAccounting.PasswordHashing;

namespace FinanceAccounting.Services;

public class UsersService : IUsersService
{
    private readonly ApplicationContext _ctx;

    public UsersService(ApplicationContext ctx)
    {
        _ctx = ctx;
    }
    
    /// <inheritdoc cref="IUsersService.GetList()"/>
    public async Task<List<User>> GetList()
    {
        var users = await _ctx.Users.ToListAsync();
        
        return users;
    }
    
    /// <inheritdoc cref="IUsersService.Get(int)"/>
    public async Task<User> Get(int id)
    {
        
        var user = _ctx.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
            throw new UserNotFoundException();
        
        return user;
    }

    /// <inheritdoc cref="IUsersService.Update(int, UserUpdateData)"/>
    public async Task Update(int id, UserUpdateData userUpdateData)
    {
        var user = _ctx.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
            throw new UserNotFoundException();

        if (_ctx.Users.SingleOrDefault(x => x.Email == userUpdateData.Email && x.Id != id) != null)
            throw new ExistingLoginException();
        user.Email = userUpdateData.Email;
        user.Password = HashPassword(userUpdateData.Password);
        if (_ctx.Users.SingleOrDefault(x => x.Login == userUpdateData.Login && x.Id != id) != null)
            throw new ExistingLoginException();
        user.Login = userUpdateData.Login;
        user.UpdatedAt = DateTime.Now;
        _ctx.Users.Update(user);
        await _ctx.SaveChangesAsync();
    }
    
    /// <inheritdoc cref="IUsersService.Delete(int)"/>
    public async Task Delete(int id)
    {
        var user = _ctx.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
            throw new UserNotFoundException();
        _ctx.Users.Remove(user);
        await _ctx.SaveChangesAsync();
    }
}