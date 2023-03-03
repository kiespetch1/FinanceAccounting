namespace FinanceAccounting.SearchContexts;

/// <summary>
/// Represents the data needed to search for expenses.
/// </summary>
public class ExpenseSearchContext
{
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
}