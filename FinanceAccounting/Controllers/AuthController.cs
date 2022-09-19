using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FinanceAccounting.Controllers;

public class AuthController : Controller
{

    [Route("Auth")]
    [HttpPost]
    public async Task<IActionResult> Authorize(AuthData user)
    {
        await using var ctx = new ApplicationContext();
        var isMatch = ctx.Users.FirstOrDefault(x => x.Password == user.Password && x.Email == user.Email) != null;

        try
        {
            if (isMatch == false)
            {
                throw new Exception("Wrong password or login.");
            }
            var claims = new List<Claim> {new(ClaimTypes.Name, user.Email) };
                var jwt = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                
            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
     
        }
        catch (Exception e)
        {
            return (BadRequest(e.Message));

        }
    }

    [Route("Registration")]
    [HttpPost]
    public async Task<IActionResult> Register(string login, string name, string email, string middleName, string lastName,
      string birthDate, string password)
    {
        await using var ctx = new ApplicationContext();
        var dbNameRepeat = ctx.Users.Count(x => x.Login == login) == 1;
        
        try
        {
            if (dbNameRepeat == true)
            {
                throw new Exception("This login is taken. Pick another one.");
            }
            else
            {
                var now = DateTime.Today;
                var creationDate = now;
                var editDate = now;
                
                var newUser = new User 
                    {
                        Login = login, 
                        Name = name, 
                        Email = email, 
                        MiddleName = middleName, 
                        LastName = lastName, 
                        BirthDate = Convert.ToDateTime(birthDate), 
                        Password = password,  
                        CreationDate = creationDate, 
                        EditDate = editDate
                    };
                ctx.Users.Add(newUser);
                await ctx.SaveChangesAsync();
                
                return (StatusCode(201));
            }
        }
        catch(Exception e)
        {
           return (BadRequest(e.Message));
        }
    }
}