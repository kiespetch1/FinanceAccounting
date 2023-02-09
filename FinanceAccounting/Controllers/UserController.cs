using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
// ReSharper disable RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute

namespace FinanceAccounting.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUsersService _userService;

    public UserController(IUsersService userService)
    {
        _userService = userService;
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
    public IActionResult Get(int id)
    {
        var user = _userService.Get(id);
        return Ok(user);
    }
    
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
    public IActionResult GetList()
    {
        var allUsers = _userService.GetList();
        return Ok(allUsers);
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
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userUpdateData">Desirable new data</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="UserNotFoundException">User with this ID was not found</exception>
    /// <response code="200">Data updated successfully</response>
    /// <response code="400">User with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(AuthenticationSchemes =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Roles = "Administrator,User")]
    [HttpPut]
    public IActionResult Update([FromBody]UserUpdateData userUpdateData)
    {
        var id = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        _userService.Update(id, userUpdateData);
        return Ok();
    }
}