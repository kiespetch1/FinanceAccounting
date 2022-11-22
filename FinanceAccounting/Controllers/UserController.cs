using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceAccounting.Exceptions;

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
    public async Task<IActionResult> GetUsersList()
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

    [Route("{id:int}")]
    [Authorize(AuthenticationSchemes =
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> GetUser(int id)
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
    [Route("{id:int}")]
    [Authorize(AuthenticationSchemes =
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(int id)
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
    /// <param name="newEmail">Desirable new login</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="UserNotFoundException">User with this ID was not found</exception>
    /// <response code="200">Email changed</response>
    /// <response code="400">User with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(AuthenticationSchemes =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Roles = "Administrator,User")]
    [HttpPut]
    public async Task<IActionResult> EditEmail([FromBody]string newEmail)
    {
        await using var ctx = new ApplicationContext();

        var id = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var currentUser = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (currentUser == null)
            throw new UserNotFoundException();
        currentUser.Email = newEmail;
        currentUser.EditDate = DateTime.Today;
        ctx.Users.Update(currentUser);
        await ctx.SaveChangesAsync();
        return Ok();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newPass">Desirable new password</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="UserNotFoundException">User with this ID was not found</exception>
    /// <response code="200">Password changed</response>
    /// <response code="400">User with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Route("edit/password")]
    [Authorize(AuthenticationSchemes = 
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,User")]
    [HttpPost]
    public async Task<IActionResult> EditPassword([FromBody]string newPass)
    {
        await using var ctx = new ApplicationContext();

        var id = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var currentUser = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (currentUser == null)
            throw new UserNotFoundException();
        currentUser.Password = newPass;
        currentUser.EditDate = DateTime.Today;
        ctx.Users.Update(currentUser);
        await ctx.SaveChangesAsync();
        return Ok();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newLogin">Desirable new login</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="UserNotFoundException">User with this ID was not found</exception>
    /// <response code="200">Login changed</response>
    /// <response code="400">User with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Route("edit/login")]
    [Authorize(AuthenticationSchemes = 
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,User")]
    [HttpPost]
    public async Task<IActionResult> EditLogin([FromBody]string newLogin)
    {
        await using var ctx = new ApplicationContext();

        var id = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var currentUser = ctx.Users.SingleOrDefault(x => x.Id == id);
        if (currentUser == null)
            throw new UserNotFoundException();
        currentUser.Login = newLogin;
        currentUser.EditDate = DateTime.Today;
        ctx.Users.Update(currentUser);
        await ctx.SaveChangesAsync();
        return Ok();
    }
}