namespace FinanceAccounting.Models;

/// <summary>
/// Represents the data required to update an income.
/// </summary>
public class IncomeUpdateData
{
    public string Name { get; set; }
    
    public float Amount { get; set; } 
    
    public int CategoryId { get; set; }
}