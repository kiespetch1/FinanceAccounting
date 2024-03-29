﻿using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models;

/// <summary>
/// Represents the data required for registration.
/// </summary>
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
    
    public string ConfirmPassword { get; set; }

}