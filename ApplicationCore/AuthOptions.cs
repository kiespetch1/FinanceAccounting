using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FinanceAccounting;

public class AuthOptions
{
    public const string Issuer = "Server";
    public const string Audience = "Client"; 
    public const string Key = "mysupersecret_secretkey!123";
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}