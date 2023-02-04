using System.Security.Claims;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers;

[ApiController]
[Route("income")]
public class IncomeSourceController : ControllerBase
{
    
    private readonly IIncomeSourceService _incomeService;

    public IncomeSourceController(IIncomeSourceService incomeService)
    {
        _incomeService = incomeService;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">Received user ID</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="UserNotFoundException">User with this ID was not found</exception>
    /// <exception cref="ExistingIncomeSourceException">Income source with given name already exists</exception>
    /// <response code="200">Success</response>
    /// <response code="400">Income source with this name already exist</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(AuthenticationSchemes =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Roles = "Administrator,User")]
    [HttpPost]
    public async Task<IActionResult> Create(string newIncomeName)
    {
        var userId = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        await _incomeService.Create(newIncomeName, userId);
        return Ok();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="IncomeSourceNotFoundException">No income sources was found</exception>
    /// <response code="200">Success</response>
    /// <response code="400">Income source with this name already exist</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(AuthenticationSchemes =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Roles = "Administrator,User")]
    [HttpGet]
    public IActionResult GetList()
    {
        var userId = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return Ok(_incomeService.GetList(userId));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">Received income source ID</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="IncomeSourceNotFoundException">Income source with this ID was not found</exception>
    /// <exception cref="NoAccessException">You don't have an access to perform this action</exception>
    /// <response code="200">Success</response>
    /// <response code="400">Income source with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("{id}")]
    [Authorize(AuthenticationSchemes =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Roles = "Administrator,User")]
    [HttpGet]
    public IActionResult Get(int id)
    {
        var userId = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return Ok(_incomeService.Get(id, userId));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">Received income source ID</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="IncomeSourceNotFoundException">No income sources was found</exception>
    /// <exception cref="NoAccessException">You don't have an access to perform this action</exception>
    /// <response code="200">Success</response>
    /// <response code="400">Income source with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("{id}")]
    [Authorize(AuthenticationSchemes =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Roles = "Administrator,User")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var userId = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        _incomeService.Delete(id, userId);
        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">Received income source ID</param>
    /// <param name="newName">Desired new name</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <exception cref="IncomeSourceNotFoundException">No income sources was found</exception>
    /// <exception cref="NoAccessException">You don't have an access to perform this action</exception>
    /// <response code="200">Success</response>
    /// <response code="400">Income source with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("{id}")]
    [Authorize(AuthenticationSchemes =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Roles = "Administrator,User")]
    [HttpPut]
    public IActionResult Update(int id, string newName)
    {
        var userId = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        _incomeService.Update(id, newName, userId);
        return Ok();
    }
    
}    