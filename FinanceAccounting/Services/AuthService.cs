using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Models;
using Microsoft.IdentityModel.Tokens;
using static FinanceAccounting.PasswordHashing;

namespace FinanceAccounting.Services;

public class AuthService
{
    public interface ILoginService
    {
        JwtSecurityToken Login(AuthData authData);
    }
    
    public class LoginService : ILoginService
    {
        public JwtSecurityToken Login(AuthData authData)
        {
            using var ctx = new ApplicationContext();
            var user = ctx.Users.SingleOrDefault(x => x.Email == authData.Email);

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
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            return jwt;
        }
    }
    
    public interface IRegistrationService
    {
        void Register(RegistrationData user);
    }
    
    public class RegistrationService : IRegistrationService
    {
        public async void Register(RegistrationData user)
        {
            await using var ctx = new ApplicationContext();

            if (ctx.Users.SingleOrDefault(x => x.Login == user.Login
                                               || x.Email == user.Email) != null)
            {
                throw new ExistingLoginException();
            }

            var now = DateTime.Today;

            var newUser = new User
            {
                Login = user.Login,
                Name = user.Name,
                Email = user.Email,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Password = HashPassword(user.Password),
                CreationDate = now,
                EditDate = now,
                Role = Role.User
            };

            ctx.Users.Add(newUser);
            await ctx.SaveChangesAsync();
        }
    }
}