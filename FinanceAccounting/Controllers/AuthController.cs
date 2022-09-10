using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FinanceAccounting.Controllers;

public class AuthController : Controller
{

    [Route("Auth")]
    [HttpGet]
    public async Task<IActionResult> Authorize(string email, string password)
    {
        await using var ctx = new MyContext();
        var isMatched = ctx.Users.Count(x => x.Password == password && x.Email == email) == 1;

        try
        {
            if (isMatched == true)
            {
                var claims = new List<Claim> {new(ClaimTypes.Name, email) };
                var jwt = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            else
            {
                throw new Exception("Wrong password or login.");
            }
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
        await using var ctx = new MyContext();
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
                
                int id;
                if (!ctx.Users.OrderBy(x=> x).Any())
                {
                    id = 1;
                }
                else
                {
                    id = ctx.Users.OrderBy(x => x.Id).Last().Id + 1;
                }
                
                var newUser = new User {Id = id, Login = login, Name = name, Email = email, MiddleName = middleName, LastName = lastName, BirthDate = Convert.ToDateTime(birthDate), Password = password,  CreationDate = creationDate, EditDate = editDate};
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