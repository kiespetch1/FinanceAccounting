﻿using FinanceAccounting.Entities;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.Services;

public class ExpenseSourceService : IExpenseSourceService
{
    
    private readonly ApplicationContext _ctx;

    public ExpenseSourceService(ApplicationContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<ExpenseSource> Create(string expenseName, int userId)
    {
        var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            throw new UserNotFoundException();
        if (await _ctx.ExpenseSources
                .SingleOrDefaultAsync(x => x.Name == expenseName && x.UserId == userId) != null)
            throw new ExistingCategoryException();
        
        var newExpenseSource = new ExpenseSource
        {
            Name = expenseName,
            UserId = userId
        };
        await _ctx.ExpenseSources.AddAsync(newExpenseSource);
        await _ctx.SaveChangesAsync();

        return newExpenseSource;
    }
    
    public async Task<List<ExpenseSource>> GetList(int userId)
    {
        var expenseSourceList = await _ctx.ExpenseSources.Where(x => x.UserId == userId).ToListAsync();
        return expenseSourceList;
    }

    public async Task<ExpenseSource> Get(int id, int userId)
    {
        var expenseSource = await _ctx.ExpenseSources.SingleOrDefaultAsync(x => x.Id == id);
        if (expenseSource == null)
            throw new CategoryNotFoundException();
        if (expenseSource.UserId != userId)
            throw new NoAccessException();

        return expenseSource;
    }

    public async Task Update(int id, string newName, int userId)
    {
        var expenseSource = _ctx.ExpenseSources.SingleOrDefault(x => x.Id == id);
        if (expenseSource == null)
            throw new CategoryNotFoundException();
        if (userId != expenseSource.UserId)
            throw new NoAccessException();
        if (_ctx.ExpenseSources.SingleOrDefault(x => x.Name == newName && x.UserId == userId) != null)
            throw new ExistingCategoryException();
        
        expenseSource.Name = newName;
        _ctx.ExpenseSources.Update(expenseSource);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task Delete(int id, int userId)
    {
        var expenseSource = await _ctx.ExpenseSources.SingleOrDefaultAsync(x => x.Id == id);
        if (expenseSource == null)
            throw new CategoryNotFoundException();
        if (expenseSource.UserId != userId)
            throw new NoAccessException();
        _ctx.ExpenseSources.Remove(expenseSource);
        await _ctx.SaveChangesAsync();
    }
}