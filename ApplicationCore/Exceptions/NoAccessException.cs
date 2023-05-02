using System.Net;

namespace PublicApi.Exceptions;

public class NoAccessException : BaseException
{
    public NoAccessException()
        : base("You do not have access to this action.", HttpStatusCode.Forbidden) { }
}