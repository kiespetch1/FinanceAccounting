using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.Services;

public class IncomeSourceService : IIncomeSourceService
{
    private readonly ApplicationContext _ctx;

    public IncomeSourceService(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IncomeSource> Create(string incomeName, int userId)
    {
        var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            throw new UserNotFoundException();
        if (await _ctx.IncomeSources.SingleOrDefaultAsync(x => x.Name == incomeName && x.UserId == userId) != null)
            throw new ExistingIncomeSourceException();
        var newIncomeSource = new IncomeSource
        {
            Name = incomeName,
            UserId = userId
        };
        await _ctx.IncomeSources.AddAsync(newIncomeSource);
        await _ctx.SaveChangesAsync();

        return (newIncomeSource);
    }

    public async Task<IncomeSource> Get(int id, int userId)
    {
        var incomeSource = await _ctx.IncomeSources.SingleOrDefaultAsync(x => x.Id == id);
        if (incomeSource == null)
            throw new IncomeSourceNotFoundException();
        if (incomeSource.UserId != userId)
            throw new NoAccessException();

        return incomeSource;
    }

    public List<IncomeSource> GetList(int userId)
    {
        var incomeSourceList = _ctx.IncomeSources.Where(x => x.UserId == userId).ToList();
        return incomeSourceList;
    }

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
}