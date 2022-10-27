using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using FinanceAccounting.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FinanceAccounting.Controllers;

public class AuthController : Controller
{
/// <summary>
/// 
/// </summary>
/// <param name="user">Email and password of the user</param>
/// <returns>User JWT Token, Status Code 200 (OK)</returns>
/// <exception cref="WrongCredentialsException"></exception>
/// <response code="200">User successfully logged in</response>
/// <response code="400">If the credentials are incorrect</response>
    [Route("Auth")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Authorize([FromBody]AuthData user)
    {
        await using var ctx = new ApplicationContext();
        
            if (ctx.Users.FirstOrDefault(x => x.Password == user.Password && x.Email == user.Email) == null)
            {
                throw new WrongCredentialsException("Wrong password or login.");
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
[Route("Registration")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
public async Task<IActionResult> Register([FromBody]RegistrationData user)
    {
        await using var ctx = new ApplicationContext();
        var dbNameRepeat = ctx.Users.SingleOrDefault(x => x.Login == user.Login) != null;
        
        
            if (dbNameRepeat == true)
            {
                throw new ExistingLoginException("This login is taken. Pick another one.");
            }
            else
            {
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
}