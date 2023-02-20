using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceAccounting.Entities;

public class Expense
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    
    public int Id { get; set; } 
    
    public string Name { get; set; } 

    [Column(TypeName = "money")] 
    public float Amount { get; set; } 

    [ForeignKey("ExpenseSource")]
    public int CategoryId { get; set; }

    [Column(TypeName = "timestamp")] 
    public DateTime CreatedAt { get; set; } 
    
    [Column(TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("User")]
    public int User { get; set; }
}