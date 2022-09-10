using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FinanceAccounting;

public class MyContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<IncomeSource> IncomeSources { get; set; }
    public MyContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Server=localhost;Port=5432;Username=postgres;Password=superuser1").UseSnakeCaseNamingConvention();
    
}