using FinanceAccounting.Controllers.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;

namespace FinanceAccounting.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : BaseController
{
    ///<summary/>
    private readonly IUsersService _userService;

    ///<summary/>
    public UserController(IUsersService userService)
    {
        _userService = userService;
    }
    
    
    /// <summary>
    /// Returns all users.
    /// </summary>
    /// <param name="page">Number of users list page</param>
    /// <returns>List of all users.</returns>
    /// <response code="200">Success.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">You don't have an access to perform this action.</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("list/{page}")]
    [Authorize(Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> GetList(int page = 1)
    {
        var allUsers = await _userService.GetList(page);
        
        return Ok(allUsers);
    }
    
    /// <summary>
    /// Returns user by ID.
    /// </summary>
    /// <param name="id">Desired user ID.</param>
    /// <returns>User with the specified ID.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">User with this ID was not found.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">You don't have an access to perform this action.</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("{id}")]
    [Authorize(Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _userService.Get(id);
        
        return Ok(user);
    }

    /// <summary>
    /// Updates current user data.
    /// </summary>
    /// <param name="userUpdateData">Desirable new user data.</param>
    /// <returns>Status code 200 (OK).</returns>
    /// <exception cref="UserNotFoundException">User with this ID was not found.</exception>
    /// <response code="204">Success.</response>
    /// <response code="400">User with this ID was not found.</response>
    /// <response code="401">Unauthorized.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(Roles = "Administrator,User")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]UserUpdateData userUpdateData)
    {
        var id = GetUserId();
        await _userService.Update(id, userUpdateData);
        
        return NoContent();
    }
    
    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">Received user ID.</param>
    /// <returns>Status code 200 (OK).</returns>
    /// <exception cref="UserNotFoundException">User with this ID was not found.</exception>
    /// <response code="204">Success.</response>
    /// <response code="400">User with this ID was not found.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">You don't have an access to perform this action.6</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("{id}")]
    [Authorize(Roles = "Administrator")]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.Delete(id);
        
        return NoContent();
    }
}