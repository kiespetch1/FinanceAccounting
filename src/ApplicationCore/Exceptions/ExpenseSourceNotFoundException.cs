using System.Net;

namespace ApplicationCore.Exceptions;

public class ExpenseSourceNotFoundException : BaseException
{  
    public ExpenseSourceNotFoundException()
        : base("Source of expense could not be found.", HttpStatusCode.BadRequest) { }
}