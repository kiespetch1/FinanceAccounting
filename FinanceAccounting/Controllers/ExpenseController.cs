using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
using FinanceAccounting.SearchContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers;

[ApiController]
[Route("expense")]

public class ExpenseController : BaseController
{

    private readonly IExpenseService _expenseService;

    public ExpenseController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// Creates a new expense.
    /// </summary>
    /// <param name="expenseCreateData">Desired expense data</param>
    /// <returns>Status Code 201 (Created)</returns>
    /// <response code="201">Success</response>
    /// <response code="400">Expense with this name already exist</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(Roles = "Administrator,User")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]ExpenseCreateData expenseCreateData)
    {
        var userId = GetUserId();
        var expense = await _expenseService.Create(userId, expenseCreateData);
        return CreatedAtAction(nameof(Create), expense);
    }
    
    /// <summary>
    /// Returns all expense for the specified period.
    /// </summary>
    /// <returns>List of the specified user's expense for a given period</returns>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(Roles = "Administrator,User")]
    [HttpGet]
    public async Task<IActionResult> GetList(ExpenseSearchContext expenseSearchContext)
    {
        var userId = GetUserId();
        var expenseList =  await _expenseService.GetList(userId, expenseSearchContext);
        
        return Ok(expenseList);
    }

    /// <summary>
    /// Returns expense by ID.
    /// </summary>
    /// <param name="id">Received expense ID</param>
    /// <returns>Requested expense</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Expense with this ID was not found</response>
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
        var expense = await _expenseService.Get(id, userId);
        
        return Ok(expense);
    }


    /// <summary>
    /// Updates expense data.
    /// </summary>
    /// <param name="expenseUpdateData">Desirable new data</param>
    /// <returns>Status Code 200 (OK)</returns>
    /// <response code="200">Data updated successfully</response>
    /// <response code="400">Expense with this ID was not found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">You don't have an access to perform this action</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [Route("{id}")]
    [Authorize(Roles = "Administrator,User")]
    [HttpPut]
    public async Task<IActionResult> Update(ExpenseUpdateData expenseUpdateData)
    {
        var userId = GetUserId();
        await _expenseService.Update(userId, expenseUpdateData);
        return Ok();
    }
    
    /// <summary>
    /// Deletes expense category.
    /// </summary>
    /// <param name="id">Received expense source ID</param>
    /// <returns>Status Code 204 (NoContent)</returns>
    /// <response code="204">Success</response>
    /// <response code="400">Expense with this ID was not found</response>
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
        await _expenseService.Delete(id, userId);
        
        return NoContent();
    }
}