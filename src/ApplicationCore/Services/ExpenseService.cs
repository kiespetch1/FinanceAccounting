using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Models.SearchContexts;
using Entities.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services;

public class ExpenseService : IExpenseService
{
    private readonly ApplicationContext _ctx;

    public ExpenseService(ApplicationContext ctx)
    {
        _ctx = ctx;
    }
    
    /// <inheritdoc cref="IExpenseService.GetList(int, CashflowSearchContext, PaginationContext, CashflowSort)"/>
    public async Task<TypeResponse<Expense>> GetList(int userId, CashflowSearchContext? expenseSearchContext, PaginationContext? expensePaginationContext, CashflowSort expenseSortOrder)
    {
        IQueryable<Expense> expense = _ctx.Expense;
        
        if (expensePaginationContext is {Page: 0})
            expensePaginationContext = new PaginationContext {Page = 1};
        
        if (expenseSearchContext.CashflowFilter != null)
        {
            if (!string.IsNullOrEmpty(expenseSearchContext.CashflowFilter.Name))
                expense = expense.Where(x => x.Name == expenseSearchContext.CashflowFilter.Name);
        
            if (expenseSearchContext.CashflowFilter.Amount is not (0 or null))
                expense = expense.Where(x => x.Amount == expenseSearchContext.CashflowFilter.Amount);

            if (expenseSearchContext.CashflowFilter.CategoryId is not (0 or null))
                expense = expense.Where(x => x.CategoryId == expenseSearchContext.CashflowFilter.CategoryId);
        }

        expense = expenseSortOrder switch
        {
            CashflowSort.AmountAsc => expense.OrderBy(x => x.Amount),
            CashflowSort.AmountDesc => expense.OrderByDescending(x => x.Amount),
            CashflowSort.NameAsc => expense.OrderBy(x => x.Name),
            CashflowSort.NameDesc => expense.OrderByDescending(x=>x.Name),
            CashflowSort.CategoryAsc => expense.OrderBy(x=>x.CategoryId),
            CashflowSort.CategoryDesc => expense.OrderByDescending(x=>x.CategoryId),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        const int pageResults = 3;
        
        var expenseList = await expense
            .Where(x => x.User == userId && x.CreatedAt >= expenseSearchContext.From && x.CreatedAt <= expenseSearchContext.To)
            .Skip((expensePaginationContext.Page - 1) * pageResults)
            .Take(pageResults)
            .ToListAsync();
        
        var response = new TypeResponse<Expense>
        {
            Items = expenseList,
            Total = expenseList.Count
        };
        
        return response;
    }
    
    /// <inheritdoc cref="IExpenseService.Get(int, int)"/>
    public async Task<Expense> Get(int id, int userId)
    {
        var expense = await _ctx.Expense.SingleOrDefaultAsync(x => x.Id == id);
        if (expense == null)
            throw new ExpenseNotFoundException();
        if (expense.User != userId)
            throw new NoAccessException();

        return expense;
    }
    
    /// <inheritdoc cref="IExpenseService.Create(int, ExpenseCreateData)"/>
    public async Task<Expense> Create(int userId, ExpenseCreateData expenseCreateData)
    {
        var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            throw new UserNotFoundException();
        if (await _ctx.ExpenseSources.SingleOrDefaultAsync(x => x.Id == expenseCreateData.CategoryId) == null)
            throw new ExpenseSourceNotFoundException();
        if (_ctx.ExpenseSources.SingleOrDefault(x => x.Id == expenseCreateData.CategoryId).UserId != userId)
            throw new NoAccessException();
        
        var newExpense = new Expense
        {
            Name = expenseCreateData.Name,
            Amount = expenseCreateData.Amount,
            CategoryId = expenseCreateData.CategoryId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            User = userId
            
        };
        await _ctx.Expense.AddAsync(newExpense);
        await _ctx.SaveChangesAsync();

        return newExpense;
    }

    /// <inheritdoc cref="IExpenseService.Update(int, int, ExpenseUpdateData)"/>
    public async Task Update(int userId, int id, ExpenseUpdateData expenseUpdateData)
    {
        var expense = _ctx.Expense.SingleOrDefault(x => x.Id == id);
        if (expense == null)
            throw new ExpenseNotFoundException();
        if (expense.User != userId)
            throw new NoAccessException();
        if (await _ctx.ExpenseSources.SingleOrDefaultAsync(x => x.Id == expenseUpdateData.CategoryId) == null)
            throw new ExpenseSourceNotFoundException();
        if (_ctx.ExpenseSources.SingleOrDefault(x => x.Id == expenseUpdateData.CategoryId).UserId != userId)
            throw new NoAccessException();
        
        expense.Name = expenseUpdateData.Name;
        expense.Amount = expenseUpdateData.Amount;
        expense.CategoryId = expenseUpdateData.CategoryId;
        expense.UpdatedAt = DateTime.Now;
        
        _ctx.Expense.Update(expense);
        await _ctx.SaveChangesAsync();
    }
    
    /// <inheritdoc cref="IExpenseService.Delete(int, int)"/>
    public async Task Delete(int id, int userId)
    {
        var expense = await _ctx.Expense.SingleOrDefaultAsync(x => x.Id == id);
        if (expense == null)
            throw new ExpenseNotFoundException();
        if (expense.User != userId)
            throw new NoAccessException();
        
        _ctx.Expense.Remove(expense);
        await _ctx.SaveChangesAsync();
    }
}