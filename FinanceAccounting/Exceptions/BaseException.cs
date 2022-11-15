using System.Net;

namespace FinanceAccounting.Exceptions;

public class BaseException : Exception
{
    public BaseException(string message, HttpStatusCode code)
        : base(message)
    {
        ErrorCode = code;
    }

    internal static HttpStatusCode ErrorCode;
}