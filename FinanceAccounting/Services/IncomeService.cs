﻿using FinanceAccounting.Exceptions;
using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.Services;

public class IncomeService : IIncomeService
{
    private readonly ApplicationContext _ctx;

    public IncomeService(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Income> Create(string incomeName, int userId, float amount, int categoryId)
    {
        var user = await _ctx.Users.SingleOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            throw new UserNotFoundException();
        if (await _ctx.IncomeSources.SingleOrDefaultAsync(x => x.Id == categoryId) == null)
            throw new CategoryNotFoundException();
        if (_ctx.IncomeSources.SingleOrDefault(x => x.Id == categoryId).UserId != userId)
            throw new NoAccessException();
        
        
        var newIncome = new Income
        {
            Name = incomeName,
            Amount = amount,
            CategoryId = categoryId,
            CreationDate = DateTime.Today,
            EditDate = DateTime.Today,
            UserId = userId
            
        };
        await _ctx.Income.AddAsync(newIncome);
        await _ctx.SaveChangesAsync();

        return (newIncome);
    }
    
    public async Task<List<Income>> GetList(int userId, DateTime from, DateTime to)
    {
        var incomeList = await _ctx.Income
            .Where(x => x.UserId == userId && x.CreationDate >= from && x.CreationDate <= to)
            .ToListAsync();
        
        return incomeList;
    }
    
    public async Task<Income> Get(int id, int userId)
    {
        var income = await _ctx.Income.SingleOrDefaultAsync(x => x.Id == id);
        if (income == null)
            throw new IncomeNotFoundException();
        if (income.UserId != userId)
            throw new NoAccessException();

        return income;
    }
    
    
    public async Task Update(int id, int userId, CategoryUpdateData categoryUpdateData)
    {
        var income = _ctx.Income.SingleOrDefault(x => x.Id == id);
        if (income == null)
            throw new IncomeNotFoundException();
        if (income.UserId != userId)
            throw new NoAccessException();
        if (await _ctx.IncomeSources.SingleOrDefaultAsync(x => x.Id == categoryUpdateData.CategoryId) == null)
            throw new CategoryNotFoundException();
        if (_ctx.IncomeSources.SingleOrDefault(x => x.Id == categoryUpdateData.CategoryId).UserId != userId)
            throw new NoAccessException();
        
        income.Name = categoryUpdateData.Name;
        income.Amount = categoryUpdateData.Amount;
        income.CategoryId = categoryUpdateData.CategoryId;
        income.EditDate = DateTime.Today;
        
        _ctx.Income.Update(income);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task Delete(int id, int userId)
    {
        var income = await _ctx.Income.SingleOrDefaultAsync(x => x.Id == id);
        if (income == null)
            throw new IncomeNotFoundException();
        if (income.UserId != userId)
            throw new NoAccessException();
        
        _ctx.Income.Remove(income);
        await _ctx.SaveChangesAsync();
    }
}