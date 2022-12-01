using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FinanceAccounting;

public class AuthOptions
{
    public const string Issuer = "https://localhost:7245/";
    public const string Audience = "MyAuthClient"; 
    public const string Key = "mysupersecret_secretkey!123";
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}