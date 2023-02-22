namespace FinanceAccounting.Models;

/// <summary>
/// Represents the data required to create an income.
/// </summary>
public class IncomeCreateData
{
    public string Name { get; set; }
    
    public float Amount { get; set; }
    
    public int CategoryId { get; set; }
}