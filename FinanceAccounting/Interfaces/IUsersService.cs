using FinanceAccounting.Entities;
using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

/// <summary>
/// Defines methods associated with users.
/// </summary>
public interface IUsersService
{

    /// <summary>
    /// Returns all users.
    /// </summary>
    /// <param name="page">Number of users list page</param>
    /// <returns>List of all users.</returns>
    Task<TypeResponse<User>> GetList(int page);
    
    /// <summary>
    /// Returns user by ID.
    /// </summary>
    /// <param name="id">Desired user ID.</param>
    /// <returns>User with the specified ID.</returns>
    Task<User> Get(int id);
    
    /// <summary>
    /// Updates current user data.
    /// </summary>
    /// <param name="id">Desired user ID.</param>
    /// <param name="userUpdateData">Desirable new user data.</param>
    Task Update(int id, UserUpdateData userUpdateData);
    
    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">Desired user ID.</param>
    Task Delete(int id);
    
}