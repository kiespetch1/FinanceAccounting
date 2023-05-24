using System.Net;

namespace ApplicationCore.Exceptions;

public class WrongCredentialsException : BaseException
{
    public WrongCredentialsException()
        : base("Wrong password or login.", HttpStatusCode.BadRequest) { }
    
}