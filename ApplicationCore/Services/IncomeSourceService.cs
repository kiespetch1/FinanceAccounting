using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Models.SearchContexts;
using Entities.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services;

public class IncomeSourceService : IIncomeSourceService
{
    private readonly ApplicationContext _ctx;

    public IncomeSourceService(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    /// <inheritdoc cref="IIncomeSourceService.GetList(int,int,CategoriesSort,CategoriesFilter)"/>
    public async Task<TypeResponse<IncomeSource>> GetList(int userId, PaginationContext? categoriesPaginationContext, CategoriesSort incomeSourceSortOrder, CategoriesFilter categoriesFilter)
    {
        IQueryable<IncomeSource> incomeSource = _ctx.IncomeSources;
        
        if (categoriesPaginationContext is {Page: 0})
            categoriesPaginationContext = new PaginationContext {Page = 1};
        
        
        if (!string.IsNullOrEmpty(categoriesFilter.Name))
            incomeSource = incomeSource.Where(x => x.Name == categoriesFilter.Name);
        
        incomeSource = incomeSourceSortOrder switch
        {
            CategoriesSort.NameAsc => incomeSource.OrderBy(x => x.Name),
            CategoriesSort.NameDesc => incomeSource.OrderByDescending(x=>x.Name),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        const int pageResults = 3;

        var incomeSourceList = await incomeSource
            .Where(x => x.UserId == userId)
            .Skip((categoriesPaginationContext.Page - 1) * pageResults)
            .Take(pageResults)
            .ToListAsync();
        
        var response = new TypeResponse<IncomeSource>
        {
            Items = incomeSourceList,
            Total = incomeSourceList.Count
        };
        
        return response;
    }

    /// <inheritdoc cref="IIncomeSourceService.Get(int, int)"/>
    public async Task<IncomeSource> Get(int id, int userId)
    {
        var incomeSource = await _ctx.IncomeSources.SingleOrDefaultAsync(x => x.Id == id);
        if (incomeSource == null)
            throw new IncomeSourceNotFoundException();
        if (incomeSource.UserId != userId)
            throw new NoAccessException();

        return incomeSource;
    }
    
    /// <inheritdoc cref="IIncomeSourceService.Create(string, int)"/>
    public async Task<IncomeSource> Create(string incomeName, int userId)
    {
        var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            throw new UserNotFoundException();
        if (await _ctx.IncomeSources
                .SingleOrDefaultAsync(x => x.Name == incomeName && x.UserId == userId) != null)
            throw new ExistingIncomeSourceException();
        var newIncomeSource = new IncomeSource
        {
            Name = incomeName,
            UserId = userId
        };
        await _ctx.IncomeSources.AddAsync(newIncomeSource);
        await _ctx.SaveChangesAsync();

        return newIncomeSource;
    }

    /// <inheritdoc cref="IIncomeSourceService.Update(int, string, int)"/>
    public async Task Update(int id, string newName, int userId)
    {
        var incomeSource = _ctx.IncomeSources.SingleOrDefault(x => x.Id == id);
        if (incomeSource == null)
            throw new IncomeSourceNotFoundException();
        if (userId != incomeSource.UserId)
            throw new NoAccessException();
        if (_ctx.IncomeSources.SingleOrDefault(x => x.Name == newName && x.UserId == userId) != null)
            throw new ExistingIncomeSourceException();
        incomeSource.Name = newName;
        _ctx.IncomeSources.Update(incomeSource);
        await _ctx.SaveChangesAsync();
    }
    
    /// <inheritdoc cref="IIncomeSourceService.Delete(int, int)"/>
    public async Task Delete(int id, int userId)
    {
        var incomeSource = await _ctx.IncomeSources.SingleOrDefaultAsync(x => x.Id == id);
        if (incomeSource == null)
            throw new IncomeSourceNotFoundException();
        if (incomeSource.UserId != userId)
            throw new NoAccessException();
        _ctx.IncomeSources.Remove(incomeSource);
        await _ctx.SaveChangesAsync();
    }
}