using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceAccounting.Models;

public class Expense
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    
    public int Id { get; set; } 
    
    public string Name { get; set; } 

    [Column(TypeName = "money")] 
    public float Amount { get; set; } 

    [ForeignKey("SourceExpense")]
    public int CategoryId { get; set; }

    [Column(TypeName = "date")] 
    public DateTime CreationDate { get; set; } 
    
    [Column(TypeName="date")]
    public DateTime EditDate { get; set; }

    [ForeignKey("User")]
    public int User { get; set; }
}