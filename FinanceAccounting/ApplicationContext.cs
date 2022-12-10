using FinanceAccounting.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting;

public class ApplicationContext: DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<IncomeSource> IncomeSources { get; set; }
    
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Server=localhost;Port=5432;Username=postgres;Password=superuser1").UseSnakeCaseNamingConvention();
    
    
}