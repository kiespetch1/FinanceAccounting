﻿namespace FinanceAccounting.Models;

public class IncomeCreateData
{
    public string Name { get; set; }
    
    public float Amount { get; set; }
    
    public int CategoryId { get; set; }
}