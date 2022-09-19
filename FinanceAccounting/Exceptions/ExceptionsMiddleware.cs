using System.Net;

namespace FinanceAccounting.Exceptions;

public class ExceptionsMiddleware
{
    private readonly RequestDelegate _next;
 
    public ExceptionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }
 
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
        
        Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = exception.Message;
            switch(exception)
            {
                case ExistingLoginException:
                    code = HttpStatusCode.BadRequest;
                    result = exception.Message;
                    break;
                case WrongCredentialsException:
                    code = HttpStatusCode.BadRequest;
                    result = exception.Message;
                    break;
            }
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)code;

            if (result == string.Empty)
            {
                result = exception.Message;
            }

            return context.Response.WriteAsync(result);
        }
        
    }
    
}