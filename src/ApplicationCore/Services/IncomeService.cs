using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Models.SearchContexts;
using Entities.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services;

public class IncomeService : IIncomeService
{
    private readonly IDatabaseContext _ctx;

    public IncomeService(IDatabaseContext ctx)
    {
        _ctx = ctx;
    }
    
    /// <inheritdoc cref="IIncomeService.GetList(int,CashflowSearchContext,int,CashflowSort,CashflowFilter)"/>
    public async Task<TypeResponse<Income>> GetList(int userId, CashflowSearchContext incomeSearchContext, PaginationContext? incomePaginationContext, CashflowSort incomeSortOrder)
    {
        IQueryable<Income> income = _ctx.Income;

        if (incomePaginationContext is {Page: 0})
            incomePaginationContext = new PaginationContext {Page = 1};

        if (incomeSearchContext.CashflowFilter != null)
        {
            if (!string.IsNullOrEmpty(incomeSearchContext.CashflowFilter.Name))
                income = income.Where(x => x.Name == incomeSearchContext.CashflowFilter.Name);

            if (incomeSearchContext.CashflowFilter.Amount is not (0 or null))
                income = income.Where(x => x.Amount == incomeSearchContext.CashflowFilter.Amount);

            if (incomeSearchContext.CashflowFilter.CategoryId is not (0 or null))
                income = income.Where(x => x.CategoryId == incomeSearchContext.CashflowFilter.CategoryId);
        }

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
        
        const int pageResults = 3;
        
        var incomeList = await income
            .Where(x => x.User == userId && x.CreatedAt >= incomeSearchContext.From && x.CreatedAt <= incomeSearchContext.To)
            .Skip((incomePaginationContext.Page - 1) * pageResults)
            .Take(pageResults)
            .ToListAsync();
        
        var response = new TypeResponse<Income>
        {
            Items = incomeList,
            Total = incomeList.Count
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