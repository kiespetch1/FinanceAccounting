﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using FinanceAccounting.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static FinanceAccounting.PasswordHashing;

namespace FinanceAccounting.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user">Email and password of the user</param>
    /// <returns>User JWT Token, Status Code 200 (OK)</returns>
    /// <exception cref="WrongCredentialsException">Credentials are incorrect</exception>
    /// <response code="200">User successfully logged in</response>
    /// <response code="400">Credentials are incorrect</response>
    [Route("login")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody] AuthData user)
    {
        await using var ctx = new ApplicationContext();
        var currentUser = ctx.Users.SingleOrDefault(x => x.Email == user.Email);

        if (currentUser == null || !VerifyHashedPassword(currentUser.Password, user.Password))
        {
            throw new WrongCredentialsException();
        }
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.NameIdentifier, currentUser.Id.ToString()),
            new(ClaimTypes.Role, currentUser.Role.ToString())
        };
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"> Login, name, email, middle name, last name, birth date and password of the user</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="ExistingLoginException">Login or email is already taken</exception>
    /// <response code="200">Registration completed successfully</response>
    /// <response code="400">Login or email is already taken</response>
    [Route("registration")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register([FromBody] RegistrationData user)
    {
        await using var ctx = new ApplicationContext();

        if (ctx.Users.SingleOrDefault(x => x.Login == user.Login) != null ||
            ctx.Users.SingleOrDefault(x => x.Email == user.Email) != null)
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

        return Ok();
    }
}