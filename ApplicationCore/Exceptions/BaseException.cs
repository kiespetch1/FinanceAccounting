using System.Net;

namespace PublicApi.Exceptions;

public class BaseException : Exception
{
    public BaseException(string message, HttpStatusCode code)
        : base(message)
    {
        ErrorCode = code;
    }

    internal static HttpStatusCode ErrorCode;
}