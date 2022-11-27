using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting;

public class AuthData
{
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
}