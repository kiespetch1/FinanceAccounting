using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceAccounting.Entities;

/// <summary>
/// Represents a user.
/// </summary>
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string MiddleName { get; set; }
    
    [Column(TypeName = "date")]
    public DateTime BirthDate { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }
    
    [Column(TypeName = "timestamp")] 
    public DateTime CreatedAt { get; set; } 
    
    [Column(TypeName="timestamp")]
    public DateTime UpdatedAt { get; set; }
    
    public Role Role { get; set; }

    public List<IncomeSource> IncomeSource { get; set; } = new();
    
}