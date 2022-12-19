using System.IdentityModel.Tokens.Jwt;
using FinanceAccounting.Models;

namespace FinanceAccounting.Interfaces;

public interface IAuthService
{
    JwtSecurityToken Login(AuthData authData);
    Task Register(RegistrationData user);
}