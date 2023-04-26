namespace Entities.SearchContexts;

/// <summary>
/// Represents the data needed to search for cashflow.
/// </summary>
public class CashflowSearchContext
{
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
}