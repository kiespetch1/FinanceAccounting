using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models;

/// <summary>
/// Represents the data needed to update the user.
/// </summary>
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