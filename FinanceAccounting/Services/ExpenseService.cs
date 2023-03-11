using FinanceAccounting.Entities;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
using FinanceAccounting.SearchContexts;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.Services;

public class ExpenseService : IExpenseService
{
    private readonly ApplicationContext _ctx;

    public ExpenseService(ApplicationContext ctx)
    {
        _ctx = ctx;
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
    
    /// <inheritdoc cref="IExpenseService.GetList(int, CashflowSearchContext)"/>
    public async Task<List<Expense>> GetList(int userId, CashflowSearchContext expenseSearchContext)
    {
        var expenseList = await _ctx.Expense
            .Where(x => x.User == userId && x.CreatedAt >= expenseSearchContext.From && x.CreatedAt <= expenseSearchContext.To)
            .ToListAsync();
        
        return expenseList;
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