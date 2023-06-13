using System.Net;

namespace ApplicationCore.Exceptions;

public class NoAccessException : BaseException
{
    public NoAccessException()
        : base("You do not have access to this action.", HttpStatusCode.Forbidden) { }
}