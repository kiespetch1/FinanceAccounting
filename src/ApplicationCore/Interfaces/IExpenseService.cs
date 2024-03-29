﻿using ApplicationCore.Models;
using ApplicationCore.Models.SearchContexts;
using Entities.Entities;

namespace ApplicationCore.Interfaces;

/// <summary>
/// Defines methods related to expenses.
/// </summary>
public interface IExpenseService
{
    /// <summary>
    /// Returns all expenses for the specified period.
    /// </summary>
    /// <param name="userId">Current user ID.</param>
    /// <param name="expenseSearchContext">Specified period of time.</param>
    /// <param name="expensePaginationContext">Number of expences list page.</param>
    /// <param name="expenseSortOrder">Sorting order.</param>
    /// <returns>List of expenses.</returns>
    Task<TypeResponse<Expense>> GetList(int userId, CashflowSearchContext? expenseSearchContext, PaginationContext? expensePaginationContext, CashflowSort expenseSortOrder);
    
    /// <summary>
    /// Returns expense by ID.
    /// </summary>
    /// <param name="id">ID of the desired expense.</param>
    /// <param name="userId">Current user ID.</param>
    /// <returns>Expense with the specified ID.</returns>
    Task<Expense> Get(int id, int userId);
    
    /// <summary>
    /// Creates a new expense.
    /// </summary>
    /// <param name="userId">Current user ID.</param>
    /// <param name="expenseCreateData">Desired expense data.</param>
    /// <returns>Created expense.</returns>
    Task<Expense> Create(int userId, ExpenseCreateData expenseCreateData);

    /// <summary>
    /// Updates expense data.
    /// </summary>
    /// <param name="userId">Current user ID.</param>
    /// <param name="id">ID of the updated expense.</param>
    /// <param name="expenseUpdateData">Expense data to update.</param>
    Task Update(int userId, int id, ExpenseUpdateData expenseUpdateData);
    
    /// <summary>
    /// Deletes expense data.
    /// </summary>
    /// <param name="id">ID of income to be deleted.</param>
    /// <param name="userId">Current user ID.</param>
    Task Delete(int id, int userId);
}