using ApplicationCore.Models;
using ApplicationCore.Models.SearchContexts;
using Entities.Entities;

namespace ApplicationCore.Interfaces;

/// <summary>
/// Defines methods related to source of expense.
/// </summary>
public interface IExpenseSourceService
{
    /// <summary>
    /// Returns all expense source categories.
    /// </summary>
    /// <param name="userId">Current user ID.</param>
    /// <param name="categoriesPaginationContext">Number of expence sources list page.</param>
    /// <param name="expenseSortOrder">Sorting order.</param>
    /// <param name="categoriesFilter">Filtering options.</param>
    /// <returns>List of expense sources of current user.</returns>
    Task<TypeResponse<ExpenseSource>> GetList(int userId, PaginationContext? categoriesPaginationContext,
        CategoriesSort expenseSortOrder, CategoriesFilter categoriesFilter);

    /// <summary>
    /// Returns expense source category by ID.
    /// </summary>
    /// <param name="id">Desired expense source ID.</param>
    /// <param name="userId">Current user ID.</param>
    /// <returns>Expense source with the specified ID.</returns>
    Task<ExpenseSource> Get(int id, int userId);
    
    /// <summary>
    /// Creates a new expense source category.
    /// </summary>
    /// <param name="expenseName">The name of the new expense.</param>
    /// <param name="userId">Current user ID.</param>
    /// <returns>Created expense source.</returns>
    Task<ExpenseSource> Create(string expenseName, int userId);

    /// <summary>
    /// Updates expense source category data.
    /// </summary>
    /// <param name="id">Received expense source ID.</param>
    /// <param name="newName">Desired new name.</param>
    /// <param name="userId">Current user ID.</param>
    Task Update(int id, string newName, int userId);

    /// <summary>
    /// Deletes expense source category.
    /// </summary>
    /// <param name="id">Desired expense source ID.</param>
    /// <param name="userId">Current user ID.</param>
    Task Delete(int id, int userId);
}