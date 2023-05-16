using Entities.Entities;

namespace ApplicationCore.Models.SearchContexts;

/// <summary>
/// Represents the data needed to search for cashflow.
/// </summary>
public class CashflowSearchContext
{
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }

    public CashflowFilter? CashflowFilter { get; set; }
}