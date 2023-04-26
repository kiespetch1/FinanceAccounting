using System.IdentityModel.Tokens.Jwt;
using Entities.Models;

namespace FinanceAccounting.Interfaces;

/// <summary>
/// Defines methods related to authorization.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a user.
    /// </summary>
    /// <param name="user">User registration data.</param>
    Task Register(RegistrationData user);
    
    /// <summary>
    /// Returns a JWT authorization token.
    /// </summary>
    /// <param name="authData">Email and password of the user.</param>
    /// <returns>User JWT token.</returns>
    JwtSecurityToken Login(AuthData authData);
}