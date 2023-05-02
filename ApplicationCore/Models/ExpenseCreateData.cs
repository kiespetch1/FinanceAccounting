namespace Entities.Models;

/// <summary>
/// Represents the data required to create an expense.
/// </summary>
public class ExpenseCreateData
{
    public string Name { get; set; }
    
    public decimal Amount { get; set; }
    
    public int CategoryId { get; set; }
}