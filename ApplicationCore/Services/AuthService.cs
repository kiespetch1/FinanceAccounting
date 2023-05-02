using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Entities.Entities;
using Entities.Models;
using FluentValidation;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;
using PublicApi.Exceptions;
using PublicApi.Interfaces;
using static PublicApi.PasswordHashing;

namespace PublicApi.Services;
public class AuthService : IAuthService
{
    private readonly ApplicationContext _ctx;
    private readonly IValidator<RegistrationData> _validator;

    public AuthService(ApplicationContext ctx, IValidator<RegistrationData> validator)
    {
        _validator = validator;
        _ctx = ctx;
    }
    
    /// <inheritdoc cref="IAuthService.Register(RegistrationData)"/>
    public async Task Register(RegistrationData user)
    {
        await _validator.ValidateAndThrowAsync(user);

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
    
    /// <inheritdoc cref="IAuthService.Login(AuthData)"/>
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