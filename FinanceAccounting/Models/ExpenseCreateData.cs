namespace FinanceAccounting.Models;

/// <summary>
/// Represents the data required to create an expense.
/// </summary>
public class ExpenseCreateData
{
    public string Name { get; set; }
    
    public float Amount { get; set; }
    
    public int CategoryId { get; set; }
}