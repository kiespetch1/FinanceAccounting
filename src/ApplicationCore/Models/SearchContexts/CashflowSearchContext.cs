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

    public CashflowSearchContext(DateTime from, DateTime to, CashflowFilter cashflowFilter)
    {
        From = from;
        To = to;
        CashflowFilter = cashflowFilter;
    }

    public CashflowSearchContext()
    {
    }
}
