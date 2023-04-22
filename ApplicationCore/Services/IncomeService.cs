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
    
    /// <inheritdoc cref="IIncomeService.GetList(int, CashflowSearchContext, int, CashflowSort)"/>
    public async Task<TypeResponse<Income>> GetList(int userId, CashflowSearchContext incomeSearchContext, int page, CashflowSort incomeSortOrder, CashflowFilter cashflowFilter)
    {
        IQueryable<Income> income = _ctx.Income;
        
        if (cashflowFilter.Name is not ("" or null))
            income = income.Where(x => x.Name == cashflowFilter.Name);
        
        if (cashflowFilter.Amount  is not (0 or null))
            income = income.Where(x => x.Amount == cashflowFilter.Amount);
        
        if (cashflowFilter.CategoryId is not (0 or null))
            income = income.Where(x => x.CategoryId == cashflowFilter.CategoryId);
        
        income = incomeSortOrder switch
        {
            CashflowSort.AmountAsc => income.OrderBy(x => x.Amount),
            CashflowSort.AmountDesc => income.OrderByDescending(x => x.Amount),
            CashflowSort.NameDesc => income.OrderByDescending(x=>x.Name),
            CashflowSort.NameAsc => income.OrderBy(x=>x.Name),
            CashflowSort.CategoryAsc => income.OrderBy(x=>x.CategoryId),
            CashflowSort.CategoryDesc => income.OrderByDescending(x=>x.CategoryId),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var pageResults = 3f;
        var pageCount = Math.Ceiling(income.Count(x=> x.Id == userId) / pageResults);
        
        var incomeList = await income
            .Where(x => x.User == userId && x.CreatedAt >= incomeSearchContext.From && x.CreatedAt <= incomeSearchContext.To)
            .Skip((page - 1) * (int)pageResults)
            .Take((int)pageResults)
            .ToListAsync();
        
        var response = new TypeResponse<Income>
        {
            TypeList = incomeList,
            CurrentPage = page,
            Pages = (int) pageCount
        };
        
        return response;
    }
    
    /// <inheritdoc cref="IIncomeService.Get(int, int)"/>
    public async Task<Income> Get(int id, int userId)
    {
        var income = await _ctx.Income.SingleOrDefaultAsync(x => x.Id == id);
        if (income == null)
            throw new IncomeNotFoundException();
        if (income.User != userId)
            throw new NoAccessException();

        return income;
    }
    
    /// <inheritdoc cref="IIncomeService.Create(int, IncomeCreateData)"/>
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

    /// <inheritdoc cref="IIncomeService.Update(int, int, IncomeUpdateData)"/>
    public async Task Update(int userId, int id, IncomeUpdateData incomeUpdateData)
    {
        var income = _ctx.Income.SingleOrDefault(x => x.Id == id);
        if (income == null)
            throw new IncomeNotFoundException();
        if (income.User != userId)
            throw new NoAccessException();
        if (await _ctx.IncomeSources.SingleOrDefaultAsync(x => x.Id == incomeUpdateData.CategoryId) == null)
            throw new IncomeSourceNotFoundException();
        if (_ctx.IncomeSources.SingleOrDefault(x => x.Id == incomeUpdateData.CategoryId).UserId != userId)
            throw new NoAccessException();
            
        income.Name = incomeUpdateData.Name;
        income.Amount = incomeUpdateData.Amount;
        income.CategoryId = incomeUpdateData.CategoryId;
        income.UpdatedAt = DateTime.Now;
        
        _ctx.Income.Update(income);
        await _ctx.SaveChangesAsync();
    }
    
    /// <inheritdoc cref="IIncomeService.Delete(int, int)"/>
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