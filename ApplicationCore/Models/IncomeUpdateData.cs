namespace Entities.Models;

/// <summary>
/// Represents the data required to update an income.
/// </summary>
public class IncomeUpdateData
{
    public string Name { get; set; }
    
    public decimal Amount { get; set; } 
    
    public int CategoryId { get; set; }
}