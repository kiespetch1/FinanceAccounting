using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting.Models;

public class AuthData
{
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
}