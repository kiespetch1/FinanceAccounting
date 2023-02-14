namespace FinanceAccounting.Models;

public class IncomeUpdateData
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public float Amount { get; set; } 
    
    public int CategoryId { get; set; }
}