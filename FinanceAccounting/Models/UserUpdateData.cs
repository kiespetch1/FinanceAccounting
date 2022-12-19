using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting.Models;

public class UserUpdateData
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string Login { get; set; }
}