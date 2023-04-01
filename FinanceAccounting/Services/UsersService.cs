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
    
    /// <inheritdoc cref="IUsersService.GetList(int, UsersSort)"/>
    public async Task<TypeResponse<User>> GetList(int page, UsersSort usersSortOrder)
    {
        var pageResults = 3f;
        var pageCount = Math.Ceiling(_ctx.Users.Count() / pageResults);

        IQueryable<User> users = _ctx.Users;
        users = usersSortOrder switch
        {
            UsersSort.EmailAsc => users.OrderBy(x => x.Email),
            UsersSort.EmailDesc => users.OrderByDescending(x => x.Email),
            UsersSort.NameDesc => users.OrderByDescending(x => x.Name),
            UsersSort.NameAsc => users.OrderBy(x => x.Name),
            UsersSort.MiddleNameAsc => users.OrderBy(x => x.MiddleName),
            UsersSort.MiddleNameDesc => users.OrderByDescending(x => x.MiddleName),
            UsersSort.LastNameAsc => users.OrderBy(x => x.LastName),
            UsersSort.LastNameDesc => users.OrderByDescending(x => x.LastName),
            UsersSort.BirthDateAsc => users.OrderBy(x => x.BirthDate),
            UsersSort.BirthDateDesc => users.OrderByDescending(x => x.BirthDate),
            _ => throw new ArgumentOutOfRangeException()
        };

        var orderedUsers = await users.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToListAsync();
        
        var response = new TypeResponse<User>
        {
            TypeList = orderedUsers,
            CurrentPage = page,
            Pages = (int) pageCount
        };

        return response;
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