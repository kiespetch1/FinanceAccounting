using FinanceAccounting.Entities;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
using FinanceAccounting.SearchContexts;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.Services;

public class IncomeService : IIncomeService
{
    private readonly ApplicationContext _ctx;

    public IncomeService(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Income> Create(int userId, IncomeCreateData incomeCreateData)
    {
        var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            throw new UserNotFoundException();
        if (await _ctx.IncomeSources.SingleOrDefaultAsync(x => x.Id == incomeCreateData.CategoryId) == null)
            throw new IncomeSourceNotFoundException();
        if (_ctx.IncomeSources.SingleOrDefault(x => x.Id == incomeCreateData.CategoryId).UserId != userId)
            throw new NoAccessException();
        
        var newIncome = new Income
        {
            Name = incomeCreateData.Name,
            Amount = incomeCreateData.Amount,
            CategoryId = incomeCreateData.CategoryId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            User = userId
            
        };
        await _ctx.Income.AddAsync(newIncome);
        await _ctx.SaveChangesAsync();

        return newIncome;
    }
    
    public async Task<List<Income>> GetList(int userId, IncomeSearchContext incomeSearchContext)
    {
        var incomeList = await _ctx.Income
            .Where(x => x.User == userId && x.CreatedAt >= incomeSearchContext.From && x.CreatedAt <= incomeSearchContext.To)
            .ToListAsync();
        
        return incomeList;
    }
    
    public async Task<Income> Get(int id, int userId)
    {
        var income = await _ctx.Income.SingleOrDefaultAsync(x => x.Id == id);
        if (income == null)
            throw new IncomeNotFoundException();
        if (income.User != userId)
            throw new NoAccessException();

        return income;
    }
    
    
    public async Task Update(int userId, IncomeUpdateData incomeUpdateData)
    {
        var income = _ctx.Income.SingleOrDefault(x => x.Id == incomeUpdateData.CategoryId);
        if (income == null)
            throw new IncomeNotFoundException();
        if (income.User != userId)
            throw new NoAccessException();
        if (_ctx.IncomeSources.SingleOrDefault(x => x.Id == incomeUpdateData.CategoryId).UserId != userId)
            throw new NoAccessException();
            
        income.Name = incomeUpdateData.Name;
        income.Amount = incomeUpdateData.Amount;
        income.CategoryId = incomeUpdateData.CategoryId;
        income.UpdatedAt = DateTime.Today;
        
        _ctx.Income.Update(income);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task Delete(int id, int userId)
    {
        var income = await _ctx.Income.SingleOrDefaultAsync(x => x.Id == id);
        if (income == null)
            throw new IncomeNotFoundException();
        if (income.User != userId)
            throw new NoAccessException();
        
        _ctx.Income.Remove(income);
        await _ctx.SaveChangesAsync();
    }
}