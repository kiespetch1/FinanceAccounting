using System.IdentityModel.Tokens.Jwt;
using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

public interface IAuthService
{
    Task Register(RegistrationData user);
    
    JwtSecurityToken Login(AuthData authData);
}