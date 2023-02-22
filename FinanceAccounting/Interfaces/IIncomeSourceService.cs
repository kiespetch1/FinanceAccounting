using FinanceAccounting.Entities;

namespace FinanceAccounting.Interfaces;

/// <summary>
/// Defines methods related to source of income.
/// </summary>
public interface IIncomeSourceService
{
    /// <summary>
    /// Creates a new income source category.
    /// </summary>
    /// <param name="incomeName">The name of the new income.</param>
    /// <param name="userId">Current user ID.</param>
    /// <returns>Created income source.</returns>
    Task<IncomeSource> Create(string incomeName, int userId);

    /// <summary>
    /// Returns all income source categories.
    /// </summary>
    /// <param name="userId">Current user ID.</param>
    /// <returns>List of income sources of current user.</returns>
    Task<List<IncomeSource>> GetList(int userId);

    /// <summary>
    /// Returns income source category by ID.
    /// </summary>
    /// <param name="id">Desired income source ID.</param>
    /// <param name="userId">Current user ID.</param>
    /// <returns>Income source with the specified ID.</returns>
    Task<IncomeSource> Get(int id, int userId);

    /// <summary>
    /// Updates income source category data.
    /// </summary>
    /// <param name="id">Received income source ID.</param>
    /// <param name="newName">Desired new name.</param>
    /// <param name="userId">Current user ID.</param>
    Task Update(int id, string newName, int userId);

    /// <summary>
    /// Deletes income source category.
    /// </summary>
    /// <param name="id">Desired income source ID.</param>
    /// <param name="userId">Current user ID.</param>
    Task Delete(int id, int userId);
}