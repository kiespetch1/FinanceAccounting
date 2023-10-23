using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities;

/// <summary>
/// Represents a source of income.
/// </summary>
public class IncomeSource : IEquatable<IncomeSource>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    
    public bool Equals(IncomeSource? other)
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