using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Models.SearchContexts;
using Entities.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services;

public class ExpenseSourceService : IExpenseSourceService
{
    
    private readonly IDatabaseContext _ctx;

    public ExpenseSourceService(IDatabaseContext ctx)
    {
        _ctx = ctx;
    }
    
    /// <inheritdoc cref="IExpenseService.GetList(int,CashflowSearchContext,PaginationContext,CashflowSort)"/>
    public async Task<TypeResponse<ExpenseSource>> GetList(int userId, PaginationContext? categoriesPaginationContext, CategoriesSort expenseSortOrder, CategoriesFilter categoriesFilter)
    {
        const int pageResults = 3;
        
        IQueryable<ExpenseSource> expenseSource = _ctx.ExpenseSources;
        
        if (categoriesPaginationContext is {Page: 0})
            categoriesPaginationContext = new PaginationContext {Page = 1};
        
        if (!string.IsNullOrEmpty(categoriesFilter.Name))
            expenseSource = expenseSource.Where(x => x.Name == categoriesFilter.Name);
        
        expenseSource = expenseSortOrder switch
        {
            CategoriesSort.NameAsc => expenseSource.OrderBy(x => x.Name),
            CategoriesSort.NameDesc => expenseSource.OrderByDescending(x=>x.Name),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var expenseSourceList = await expenseSource
            .Where(x => x.UserId == userId)
            .Skip((categoriesPaginationContext.Page - 1) * pageResults)
            .Take(pageResults)
            .ToListAsync();
        
        var response = new TypeResponse<ExpenseSource>
        {
            Items = expenseSourceList,
            Total = expenseSourceList.Count
        };
        
        return response;
    }

    /// <inheritdoc cref="IExpenseService.Get(int, int)"/>
    public async Task<ExpenseSource> Get(int id, int userId)
    {
        var expenseSource = await _ctx.ExpenseSources.SingleOrDefaultAsync(x => x.Id == id);
        if (expenseSource == null)
            throw new ExpenseSourceNotFoundException();
        if (expenseSource.UserId != userId)
            throw new NoAccessException();

        return expenseSource;
    }
    
    /// <inheritdoc cref="IExpenseService.Create(int, ExpenseCreateData)"/>
    public async Task<ExpenseSource> Create(string expenseName, int userId)
    {
        var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            throw new UserNotFoundException();
        if (await _ctx.ExpenseSources
                .SingleOrDefaultAsync(x => x.Name == expenseName && x.UserId == userId) != null)
            throw new ExistingExpenseSourceException();
        
        var newExpenseSource = new ExpenseSource
        {
            Name = expenseName,
            UserId = userId
        };
        await _ctx.ExpenseSources.AddAsync(newExpenseSource);
        await _ctx.SaveChangesAsync();

        return newExpenseSource;
    }

    /// <inheritdoc cref="IExpenseService.Update(int, int, ExpenseUpdateData)"/>
    public async Task Update(int id, string newName, int userId)
    {
        var expenseSource = _ctx.ExpenseSources.SingleOrDefault(x => x.Id == id);
        if (expenseSource == null)
            throw new ExpenseSourceNotFoundException();
        if (userId != expenseSource.UserId)
            throw new NoAccessException();
        if (_ctx.ExpenseSources.SingleOrDefault(x => x.Name == newName && x.UserId == userId) != null)
            throw new ExistingExpenseSourceException();
        
        expenseSource.Name = newName;
        _ctx.ExpenseSources.Update(expenseSource);
        await _ctx.SaveChangesAsync();
    }
    /// <inheritdoc cref="IExpenseService.Delete(int, int)"/>
    public async Task Delete(int id, int userId)
    {
        var expenseSource = await _ctx.ExpenseSources.SingleOrDefaultAsync(x => x.Id == id);
        if (expenseSource == null)
            throw new ExpenseSourceNotFoundException();
        if (expenseSource.UserId != userId)
            throw new NoAccessException();
        _ctx.ExpenseSources.Remove(expenseSource);
        await _ctx.SaveChangesAsync();
    }
}