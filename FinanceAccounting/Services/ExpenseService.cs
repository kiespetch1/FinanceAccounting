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

    public async Task<Expense> Create(int userId, IncomeCreateData incomeCreateData)
    {
        var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            throw new UserNotFoundException();
        if (await _ctx.ExpenseSources.SingleOrDefaultAsync(x => x.Id == incomeCreateData.CategoryId) == null)
            throw new CategoryNotFoundException();
        if (_ctx.ExpenseSources.SingleOrDefault(x => x.Id == incomeCreateData.CategoryId).UserId != userId)
            throw new NoAccessException();
        
        var newExpense = new Expense
        {
            Name = incomeCreateData.Name,
            Amount = incomeCreateData.Amount,
            CategoryId = incomeCreateData.CategoryId,
            CreationDate = DateTime.Today,
            EditDate = DateTime.Today,
            User = userId
            
        };
        await _ctx.Expense.AddAsync(newExpense);
        await _ctx.SaveChangesAsync();

        return newExpense;
    }
    
    public async Task<List<Expense>> GetList(int userId, IncomeSearchContext incomeSearchContext)
    {
        var expenseList = await _ctx.Expense
            .Where(x => x.User == userId && x.CreationDate >= incomeSearchContext.From && x.CreationDate <= incomeSearchContext.To)
            .ToListAsync();
        
        return expenseList;
    }
    
    public async Task<Expense> Get(int id, int userId)
    {
        var expense = await _ctx.Expense.SingleOrDefaultAsync(x => x.Id == id);
        if (expense == null)
            throw new IncomeNotFoundException();
        if (expense.User != userId)
            throw new NoAccessException();

        return expense;
    }
    
    
    public async Task Update(int userId, IncomeUpdateData expenseUpdateData)
    {
        var expense = _ctx.Expense.SingleOrDefault(x => x.Id == expenseUpdateData.Id);
        if (expense == null)
            throw new ExpenseNotFoundException();
        if (expense.User != userId)
            throw new NoAccessException();
        if (await _ctx.ExpenseSources.SingleOrDefaultAsync(x => x.Id == expenseUpdateData.CategoryId) == null)
            throw new CategoryNotFoundException();
        if (_ctx.ExpenseSources.SingleOrDefault(x => x.Id == expenseUpdateData.CategoryId).UserId != userId)
            throw new NoAccessException();
        
        expense.Name = expenseUpdateData.Name;
        expense.Amount = expenseUpdateData.Amount;
        expense.CategoryId = expenseUpdateData.CategoryId;
        expense.EditDate = DateTime.Today;
        
        _ctx.Expense.Update(expense);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task Delete(int id, int userId)
    {
        var expense = await _ctx.Expense.SingleOrDefaultAsync(x => x.Id == id);
        if (expense == null)
            throw new IncomeNotFoundException();
        if (expense.User != userId)
            throw new NoAccessException();
        
        _ctx.Expense.Remove(expense);
        await _ctx.SaveChangesAsync();
    }
}