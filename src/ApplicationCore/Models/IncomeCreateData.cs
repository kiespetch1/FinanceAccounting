namespace ApplicationCore.Models;

/// <summary>
/// Represents the data required to create an income.
/// </summary>
public class IncomeCreateData
{
    public string Name { get; set; }
    
    public decimal Amount { get; set; }
    
    public int CategoryId { get; set; }
}