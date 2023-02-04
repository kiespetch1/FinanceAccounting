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
            HttpStatusCode code;
            string result;
            switch(exception)
            {
                case BaseException:
                    result = $"{exception.Message} {exception.StackTrace}";
                    code = BaseException.ErrorCode;
                    break;
                
                default:
                    result = $"Internal server error. \n {exception.Message} {exception.StackTrace}";
                    code = HttpStatusCode.InternalServerError;
                    break;
                    
            }
            
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
        
    }
    
}