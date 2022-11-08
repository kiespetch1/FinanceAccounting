using System.Net;

namespace FinanceAccounting.Exceptions;

public class WrongCredentialsException : BaseException
{
    public WrongCredentialsException()
        : base("Wrong password or login.", HttpStatusCode.BadRequest) { }
    
}