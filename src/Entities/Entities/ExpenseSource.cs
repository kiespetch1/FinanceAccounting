using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities;

/// <summary>
/// Represents the source of the expense.
/// </summary>
public class ExpenseSource : IEquatable<ExpenseSource>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }

    public bool Equals(ExpenseSource? other)
    {
        if (other == null)
            return false;

        if (Id != other.Id)
            return false;

        if (Name != other.Name)
            return false;

        if (UserId != other.UserId)
            return false;

        return true;
    }
}