using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models;

/// <summary>
/// Represents login information.
/// </summary>
public class AuthData
{
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
}