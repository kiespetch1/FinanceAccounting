using System.Net;

namespace ApplicationCore.Exceptions;

public class ExpenseNotFoundException: BaseException
{  
    public ExpenseNotFoundException()
        : base("The expense could not be found.", HttpStatusCode.BadRequest) { }
}
