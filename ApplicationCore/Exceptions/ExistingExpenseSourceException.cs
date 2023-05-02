using System.Net;

namespace PublicApi.Exceptions;

public class ExistingExpenseSourceException : BaseException
{
    public ExistingExpenseSourceException()
        : base("Expense source with given name already exists. Pick another name.", HttpStatusCode.BadRequest) { }
}