﻿using FinanceAccounting.Entities;
using FinanceAccounting.Models;
using FinanceAccounting.SearchContexts;

namespace FinanceAccounting.Interfaces;

/// <summary>
/// Defines methods related to income.
/// </summary>
public interface IIncomeService
{
    /// <summary>
    /// Creates a new income.
    /// </summary>
    /// <param name="userId">Current user ID.</param>
    /// <param name="incomeCreateData">Desired income data.</param>
    /// <returns>Created income.</returns>
    Task<Income> Create(int userId, IncomeCreateData incomeCreateData);
    
    /// <summary>
    /// Returns all income for the specified period.
    /// </summary>
    /// <param name="userId">Current user ID.</param>
    /// <param name="incomeSearchContext">Specified period of time.</param>
    /// <returns>List of the specified user's income for a given period.</returns>
    Task<List<Income>> GetList(int userId, IncomeSearchContext incomeSearchContext);
    
    /// <summary>
    /// Returns income by ID.
    /// </summary>
    /// <param name="id">ID of the desired income.</param>
    /// <param name="userId">Current user ID.</param>
    /// <returns>List of income.</returns>
    Task<Income> Get(int id, int userId);

    /// <summary>
    /// Updates income data.
    /// </summary>
    /// <param name="userId">Current user ID.</param>
    /// <param name="id">Desired income ID.</param>
    /// <param name="incomeUpdateData">Income data to update.</param>
    Task Update(int userId, int id, IncomeUpdateData incomeUpdateData);
    
    /// <summary>
    /// Deletes income category.
    /// </summary>
    /// <param name="id">Desired income ID.</param>
    /// <param name="userId">Current user ID.</param>
    Task Delete(int id, int userId);
}