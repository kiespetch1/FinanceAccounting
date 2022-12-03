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

        var user = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
            throw new UserNotFoundException();
        return Ok(user);
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

        var user = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
            throw new UserNotFoundException();
        ctx.Users.Remove(user);
        await ctx.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newUserData">Desirable new data</param>
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
    public async Task<IActionResult> Update([FromBody]UserUpdateData newUserData)
    {
        await using var ctx = new ApplicationContext();
        
        var id = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var user = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (user == null)
            throw new UserNotFoundException();
        
        if (ctx.Users.SingleOrDefault(x => x.Email == newUserData.Email && x.Id != id) != null)
            throw new ExistingLoginException();
        user.Email = newUserData.Email;
        user.Password = HashPassword(newUserData.Password);
        if (ctx.Users.SingleOrDefault(x => x.Login == newUserData.Login && x.Id != id) != null) 
            throw new ExistingLoginException();
        user.Login = newUserData.Login;
        user.EditDate = DateTime.Today;
        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();

        return Ok();
    }
}