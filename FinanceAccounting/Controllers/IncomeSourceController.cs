using FinanceAccounting.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers;

[ApiController]
[Route("income-source")]

public class IncomeSourceController : BaseController
{
    
    private readonly IIncomeSourceService _incomeSourceService;

    public IncomeSourceController(IIncomeSourceService incomeSourceService)
    {
        _incomeSourceService = incomeSourceService;
    }

    /// <summary>
    /// Creates a new income source category.
    /// </summary>
    /// <param name="newIncomeName"></param>
    /// <returns>Status Code 201 (Created)</returns>
    /// <response code="201">Success</response>
    /// <response code="400">Income source with this name already exist</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(Roles = "Administrator,User")]
    [HttpPost]
    public async Task<IActionResult> Create(string newIncomeName)
    {
        var userId = GetUserId();
        var incomeSource = await _incomeSourceService.Create(newIncomeName, userId);
        return CreatedAtAction(nameof(Create), incomeSource);
    }
    
    /// <summary>
    /// Returns all income source categories.
    /// </summary>
    /// <returns>List of income sources of current user</returns>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(Roles = "Administrator,User")]
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var userId = GetUserId();
        var incomeSourcesList =  await _incomeSourceService.GetList(userId);
        
        return Ok(incomeSourcesList);
    }
    
    /// <summary>
    /// Returns income source category by ID.
    /// </summary>
    /// <param name="id">Received income source ID</param>
    /// <returns>Requested income source category</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Income source with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("{id}")]
    [Authorize(Roles = "Administrator,User")]
    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var userId = GetUserId();
        var incomeSource = await _incomeSourceService.Get(id, userId);
        
        return Ok(incomeSource);
    }

    /// <summary>
    /// Updates income source category data.
    /// </summary>
    /// <param name="id">Received income source ID</param>
    /// <param name="newName">Desired new name</param>
    /// <returns>Status Code 204 (NoContent)</returns>
    /// <response code="204">Success</response>
    /// <response code="400">Income source with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("{id}")]
    [Authorize(Roles = "Administrator,User")]
    [HttpPut]
    public async Task<IActionResult> Update(int id, string newName)
    {
        var userId = GetUserId();
        await _incomeSourceService.Update(id, newName, userId);
        
        return NoContent();
    }
    
    /// <summary>
    /// Deletes income source category.
    /// </summary>
    /// <param name="id">Received income source ID</param>
    /// <returns>Status Code 204 (NoContent)</returns>
    /// <response code="204">Success</response>
    /// <response code="400">Income source with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("{id}")]
    [Authorize(Roles = "Administrator,User")]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        await _incomeSourceService.Delete(id, userId);
        
        return NoContent();
    }
    
}    