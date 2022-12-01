using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceAccounting.Exceptions;
using static FinanceAccounting.PasswordHashing;

namespace FinanceAccounting.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Status Code 200 (OK)</returns>
    /// <response code="200">All users data are displayed</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Authorize(AuthenticationSchemes =
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        await using var ctx = new ApplicationContext();

        var allUsers = ctx.Users.ToList();

        return Ok(allUsers);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">Received user ID</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="UserNotFoundException">User with this ID was not found</exception>
    /// <response code="200">User data with given id are displayed</response>
    /// <response code="400">User with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]

    [Route("{id}")]
    [Authorize(AuthenticationSchemes =
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        await using var ctx = new ApplicationContext();

        var currentUser = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (currentUser == null)
            throw new UserNotFoundException();
        return Ok(currentUser);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">Received user ID</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="UserNotFoundException">User with this ID was not found</exception>
    /// <response code="200">User with this ID deleted</response>
    /// <response code="400">User with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("{id}")]
    [Authorize(AuthenticationSchemes =
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await using var ctx = new ApplicationContext();

        var currentUser = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (currentUser == null)
            throw new UserNotFoundException();
        ctx.Users.Remove(currentUser);
        await ctx.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user">Desirable new data</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="UserNotFoundException">User with this ID was not found</exception>
    /// <response code="200">Data changed</response>
    /// <response code="400">User with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(AuthenticationSchemes =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Roles = "Administrator,User")]
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody]UserUpdateData user)
    {
        await using var ctx = new ApplicationContext();
        
        var id = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var currentUser = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (currentUser == null)
            throw new UserNotFoundException();

        var changes = 0;
        if (user.Email != currentUser.Email && user.Email != string.Empty) 
        {
            if (ctx.Users.SingleOrDefault(x => x.Email == user.Email) != null)
                throw new ExistingLoginException();
            currentUser.Email = user.Email;
            changes++;
        }

        if (!VerifyHashedPassword(currentUser.Password, user.Password) && user.Password != string.Empty)
        {
            currentUser.Password = HashPassword(user.Password);
            changes++;
        }

        if (user.Login != currentUser.Login && user.Login != string.Empty)
        {
            if (ctx.Users.SingleOrDefault(x => x.Login == user.Login) != null) 
                throw new ExistingLoginException();
            currentUser.Login = user.Login;
            changes++;
        }

        if (changes == 0)
            throw new ArgumentCannotBeNullException();

        currentUser.EditDate = DateTime.Today;
        ctx.Users.Update(currentUser);
        await ctx.SaveChangesAsync();

        return Ok();
    }
}