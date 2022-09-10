using System.ComponentModel.DataAnnotations.Schema;
using Npgsql;

namespace FinanceAccounting;


public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    [Column(TypeName="date")]
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    [Column(TypeName="date")]
    public DateTime CreationDate { get; set; }
    [Column(TypeName="date")]
    public DateTime EditDate { get; set; }

    public List<IncomeSource> IncomeSource { get; set; } = new();
    
    
    void Registration(string login, string name, string email, string middleName, string lastName, DateTime birthDate,string password)
    {
        this.Login = login;
        this.Name = name;
        this.Email = email;
        this.MiddleName = middleName;
        this.LastName = lastName;
        this.BirthDate = birthDate;
        this.Password = password;
        var now = DateTime.UtcNow;
        CreationDate = now;
        EditDate = now;
    }

   
}