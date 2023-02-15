using System.Net;

namespace FinanceAccounting.Exceptions;

public class CategoryNotFoundException : BaseException
{  
    public CategoryNotFoundException()
        : base("The category could not be found.", HttpStatusCode.BadRequest) { }
}