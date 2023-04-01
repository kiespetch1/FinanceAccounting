﻿namespace FinanceAccounting.Models;

/// <summary>
/// Represents the data required to update an expense.
/// </summary>
public class ExpenseUpdateData
{
    public string Name { get; set; }
    
    public float Amount { get; set; } 
    
    public int CategoryId { get; set; }
}