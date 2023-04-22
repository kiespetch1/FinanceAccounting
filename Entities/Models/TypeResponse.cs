namespace FinanceAccounting.Models;

public class TypeResponse<T>
{
    public List<T> TypeList { get; set; } = new List<T>();
    
    public int Pages { get; set; }

    public int CurrentPage { get; set; }
}