namespace FinanceAccounting.Models;

public class ExpenseCreateData
{
    public string Name { get; set; }
    
    public float Amount { get; set; }
    
    public int CategoryId { get; set; }
}