using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Database context class
/// </summary>
public interface IDatabaseContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<IncomeSource> IncomeSources { get; set; }
    
    public DbSet<Income> Income { get; set; }
    
    public DbSet<ExpenseSource> ExpenseSources { get; set; }
    
    public DbSet<Expense> Expense { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}