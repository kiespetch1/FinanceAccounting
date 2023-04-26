using Entities.Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using Infrastructure;
using static FinanceAccounting.PasswordHashing;

namespace FinanceAccounting.Services;

public class UsersService : IUsersService
{
    private readonly ApplicationContext _ctx;

    public UsersService(ApplicationContext ctx)
    {
        _ctx = ctx;
    }
    
    /// <inheritdoc cref="IUsersService.GetList(int, UsersSort, UsersFilter)"/>
    public async Task<TypeResponse<User>> GetList(int page, UsersSort usersSortOrder, UsersFilter? usersFilter)
    {
        const int pageResults = 3;

        IQueryable<User> users = _ctx.Users;

        if (usersFilter != null)
        {
            if (!string.IsNullOrEmpty(usersFilter.Name))
                users = users.Where(x => x.Name == usersFilter.Name);
        
            if (!string.IsNullOrEmpty(usersFilter.MiddleName))
                users = users.Where(x => x.MiddleName == usersFilter.MiddleName);
        
            if (!string.IsNullOrEmpty(usersFilter.LastName))
                users = users.Where(x => x.LastName == usersFilter.LastName);
        
            if (usersFilter.BirthDate != new DateTime(1,1,1,0,0,0))
                users = users.Where(x => x.BirthDate == usersFilter.BirthDate);
        
            if (!string.IsNullOrEmpty(usersFilter.Email))
                users = users.Where(x => x.Email == usersFilter.Email);
        }
        
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

        var orderedUsers = await users.Skip((page - 1) * pageResults).Take(pageResults).ToListAsync();
        
        var response = new TypeResponse<User>
        {
            Items = orderedUsers,
            Total = _ctx.Users.Count()
        };

        return response;
    }
    
    /// <inheritdoc cref="IUsersService.Get(int)"/>
    public async Task<User> Get(int id)
    {
        var user = _ctx.Users.SingleOrNotFound(x => x.Id == id);
        
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