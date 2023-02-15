﻿using FinanceAccounting.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting;

public class ApplicationContext: DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<IncomeSource> IncomeSources { get; set; }
    
    public DbSet<Income> Income { get; set; }
    
    public DbSet<ExpenseSource> ExpenseSources { get; set; }
    
    public DbSet<Expense> Expense { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    


}