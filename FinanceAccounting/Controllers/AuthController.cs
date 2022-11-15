using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinanceAccounting.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
/// <exception cref="WrongCredentialsException"></exception>
/// <response code="200">User successfully logged in</response>
/// <response code="400">If the credentials are incorrect</response>
    [Route("login")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody]AuthData user)
    {
        await using var ctx = new ApplicationContext();
        
        if (ctx.Users.SingleOrDefault(x => x.Password == user.Password && x.Email == user.Email) == null)
        {
            throw new WrongCredentialsException();
        }
        var claims = new List<Claim> {new(ClaimTypes.Name, user.Email) };
        var jwt = new JwtSecurityToken(
            claims: claims,
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                
        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

/// <summary>
/// 
/// </summary>
/// <param name="user"> Login, name, email, middle name, last name, birth date and password of the user</param>
/// <returns>Status Code 200 (OK)</returns>
/// <exception cref="ExistingLoginException"></exception>
/// <response code="200">Registration completed successfully</response>
/// <response code="400">If the login is already taken</response>
    [Route("registration")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register([FromBody]RegistrationData user)
    {
        await using var ctx = new ApplicationContext();
            
        if (ctx.Users.SingleOrDefault(x => x.Login == user.Login) != null || ctx.Users.SingleOrDefault(x => x.Email == user.Email) != null)
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
            BirthDate = Convert.ToDateTime(user.BirthDate), 
            Password = user.Password,  
            CreationDate = now, 
            EditDate = now
        };
                    
        ctx.Users.Add(newUser);
        await ctx.SaveChangesAsync();
                    
        return Ok();
    }
}