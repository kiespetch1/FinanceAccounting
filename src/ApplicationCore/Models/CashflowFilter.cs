namespace Entities.Entities;

public class CashflowFilter
{
    public string? Name { get; set; } 
    
    public decimal? Amount { get; set; } 
    
    public int? CategoryId { get; set; }

    public CashflowFilter(string name, decimal? amount, int? categoryId)
    {
        Name = name;
        Amount = amount;
        CategoryId = categoryId;
    }

    public CashflowFilter()
    {
    }
}