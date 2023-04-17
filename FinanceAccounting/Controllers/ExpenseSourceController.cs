using FinanceAccounting.Controllers.Abstractions;
using FinanceAccounting.Entities;
using FinanceAccounting.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers;

[ApiController]
[Route("expense-source")]

public class ExpenseSourceController : BaseController
{
    ///<summary/>
    private readonly IExpenseSourceService _expenseSourceService;

    ///<summary/>
    public ExpenseSourceController(IExpenseSourceService expenseSourceService)
    {
        _expenseSourceService = expenseSourceService;
    }
    
    /// <summary>
    /// Returns all expense source categories.
    /// </summary>
    /// <param name="page">Number of expense sources list page.</param>
    /// <param name="sortingOrder">Sorting order.</param>
    /// <returns>List of expense sources of current user.</returns>
    /// <response code="200">Success.</response>
    /// <response code="401">Unauthorized.</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Route("list")]
    [Authorize(Roles = "Administrator,User")]
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery]CategoriesFilter categoriesFilter, int page = 1, CategoriesSort sortingOrder = CategoriesSort.NameAsc)
    {
        var userId = GetUserId();
        var expenseSourcesList =  await _expenseSourceService.GetList(userId, page, sortingOrder, categoriesFilter);
        
        return Ok(expenseSourcesList);
    }
    
    /// <summary>
    /// Returns expense source category by ID.
    /// </summary>
    /// <param name="id">Received expense source ID.</param>
    /// <returns>Requested expense source category.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Expense source with this ID was not found.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">You don't have an access to perform this action.</response>
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
        var expenseSource = await _expenseSourceService.Get(id, userId);
        
        return Ok(expenseSource);
    }
    
    /// <summary>
    /// Creates a new expense source category.
    /// </summary>
    /// <param name="newExpenseName">The name of the new expense.</param>
    /// <returns>Status code 201 (Created).</returns>
    /// <response code="201">Success.</response>
    /// <response code="400">Expense source with this name already exist.</response>
    /// <response code="401">Unauthorized.</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(Roles = "Administrator,User")]
    [HttpPost]
    public async Task<IActionResult> Create(string newExpenseName)
    {
        var userId = GetUserId();
        var expenseSource = await _expenseSourceService.Create(newExpenseName, userId);
        
        return CreatedAtAction(nameof(Create), expenseSource);
    }

    /// <summary>
    /// Updates expense source category data.
    /// </summary>
    /// <param name="id">Received expense source ID.</param>
    /// <param name="newName">Desired new name.</param>
    /// <returns>Status code 204 (NoContent).</returns>
    /// <response code="204">Success.</response>
    /// <response code="400">Expense source with this ID was not found.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">You don't have an access to perform this action.</response>
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
        await _expenseSourceService.Update(id, newName, userId);
        
        return NoContent();
    }
    
    /// <summary>
    /// Deletes expense source category.
    /// </summary>
    /// <param name="id">Received expense source ID.</param>
    /// <returns>Status code 204 (NoContent).</returns>
    /// <response code="204">Success.</response>
    /// <response code="400">Expense source with this ID was not found.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">You don't have an access to perform this action.</response>
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
        await _expenseSourceService.Delete(id, userId);
        
        return NoContent();
    }
}