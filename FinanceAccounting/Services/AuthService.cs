using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
using Microsoft.IdentityModel.Tokens;
using static FinanceAccounting.PasswordHashing;

namespace FinanceAccounting.Services;
public class AuthService : IAuthService
{
    private readonly ApplicationContext _ctx;

    public AuthService(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Register(RegistrationData user)
    {
        if (_ctx.Users.SingleOrDefault(x => x.Login == user.Login
                                           || x.Email == user.Email) != null)
        {
            throw new ExistingLoginException();
        }

        var newUser = new User
        {
            Login = user.Login,
            Name = user.Name,
            Email = user.Email,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            BirthDate = user.BirthDate,
            Password = HashPassword(user.Password),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Role = Role.User
        };

        _ctx.Users.Add(newUser);
        await _ctx.SaveChangesAsync();
    }
    
    public JwtSecurityToken Login(AuthData authData)
    {
        var user = _ctx.Users.SingleOrDefault(x => x.Email == authData.Email);

        if (user == null || !VerifyHashedPassword(user.Password, authData.Password))
        {
            throw new WrongCredentialsException();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, authData.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString())
        };
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        return jwt;
    }
    
}