using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities;

/// <summary>
/// Represents the income.
/// </summary>
public class Income : IEquatable<Income>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    
    public int Id { get; set; } 
    
    public string Name { get; set; } 

    [Column(TypeName = "money")] 
    public decimal Amount { get; set; } 

    [ForeignKey("IncomeSource")]
    public int CategoryId { get; set; }

    [Column(TypeName = "timestamp")] 
    public DateTime CreatedAt { get; set; } 
    
    [Column(TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("User")]
    public int User { get; set; }
    
    public bool Equals(Income? other)
    {
        if (other == null)
            return false;

        if (Amount != other.Amount)
            return false;

        if (Id != other.Id)
            return false;

        if (Name != other.Name)
            return false;

        if (CategoryId != other.CategoryId)
            return false;

        if (User != other.User)
            return false;

        if (CreatedAt.Year != other.CreatedAt.Year || CreatedAt.Month != other.CreatedAt.Month ||
            CreatedAt.Day != other.CreatedAt.Day ||
            CreatedAt.Hour != other.CreatedAt.Hour || CreatedAt.Minute != other.CreatedAt.Minute ||
            CreatedAt.Second != other.CreatedAt.Second)
            return false;

        if (UpdatedAt.Year != other.UpdatedAt.Year || UpdatedAt.Month != other.UpdatedAt.Month ||
            UpdatedAt.Day != other.UpdatedAt.Day ||
            UpdatedAt.Hour != other.UpdatedAt.Hour || UpdatedAt.Minute != other.UpdatedAt.Minute ||
            UpdatedAt.Second != other.UpdatedAt.Second)
            return false;
        
        return true;
    }
}