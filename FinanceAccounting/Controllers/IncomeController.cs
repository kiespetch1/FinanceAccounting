using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
using FinanceAccounting.SearchContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers;

[ApiController]
[Route("income")]
public class IncomeController : BaseController
{

    private readonly IIncomeService _incomeService;

    public IncomeController(IIncomeService incomeService)
    {
        _incomeService = incomeService;
    }

    /// <summary>
    /// Creates a new income.
    /// </summary>
    /// <param name="incomeCreateData">Desired income data</param>
    /// <returns>Status Code 201 (Created)</returns>
    /// <response code="201">Success</response>
    /// <response code="400">Income with this name already exist</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(Roles = "Administrator,User")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]IncomeCreateData incomeCreateData)
    {
        var userId = GetUserId();
        var income = await _incomeService.Create(userId, incomeCreateData);
        return CreatedAtAction(nameof(Create), income);
    }
    
    /// <summary>
    /// Returns all income for the specified period.
    /// </summary>
    /// <returns>List of the specified user's income for a given period</returns>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(Roles = "Administrator,User")]
    [HttpGet]
    public async Task<IActionResult> GetList(IncomeSearchContext incomeSearchContext)
    {
        var userId = GetUserId();
        var incomeList =  await _incomeService.GetList(userId, incomeSearchContext);
        
        return Ok(incomeList);
    }

    /// <summary>
    /// Returns income by ID.
    /// </summary>
    /// <param name="id">Received income ID</param>
    /// <returns>Requested income</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Income with this ID was not found</response>
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
        var income = await _incomeService.Get(id, userId);
        
        return Ok(income);
    }
    
    
    /// <summary>
    /// Updates income data.
    /// </summary>
    /// <param name="incomeUpdateData">Desirable new data</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <response code="200">Data updated successfully</response>
    /// <response code="400">Income with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Authorize(Roles = "Administrator,User")]
    [HttpPut]
    public async Task<IActionResult> Update(IncomeUpdateData incomeUpdateData)
    {
        var userId = GetUserId();
        await _incomeService.Update(userId, incomeUpdateData);
        return Ok();
    }
    
    /// <summary>
    /// Deletes income category.
    /// </summary>
    /// <param name="id">Received income source ID</param>
    /// <returns>Status Code 204 (NoContent)</returns>
    /// <response code="204">Success</response>
    /// <response code="400">Income with this ID was not found</response>
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
        await _incomeService.Delete(id, userId);
        
        return NoContent();
    }
}