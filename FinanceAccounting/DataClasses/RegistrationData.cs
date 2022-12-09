using System.ComponentModel.DataAnnotations;

namespace FinanceAccounting;

public class RegistrationData
{
    public string Login { get; set; }
    
    public string Name { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    public string MiddleName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime BirthDate { get; set; } 
    
    public string Password { get; set; }

}