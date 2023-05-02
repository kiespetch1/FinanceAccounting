using System.Net;

namespace PublicApi.Exceptions;

public class WrongCredentialsException : BaseException
{
    public WrongCredentialsException()
        : base("Wrong password or login.", HttpStatusCode.BadRequest) { }
    
}