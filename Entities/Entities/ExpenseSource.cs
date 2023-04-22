using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceAccounting.Entities;

/// <summary>
/// Represents the source of the expense.
/// </summary>
public class ExpenseSource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
}