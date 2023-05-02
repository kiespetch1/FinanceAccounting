using System.Net;

namespace PublicApi.Exceptions;

public class ExpenseSourceNotFoundException : BaseException
{  
    public ExpenseSourceNotFoundException()
        : base("Source of expense could not be found.", HttpStatusCode.BadRequest) { }
}