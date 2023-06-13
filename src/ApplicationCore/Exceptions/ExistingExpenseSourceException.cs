using System.Net;

namespace ApplicationCore.Exceptions;

public class ExistingExpenseSourceException : BaseException
{
    public ExistingExpenseSourceException()
        : base("Expense source with given name already exists. Pick another name.", HttpStatusCode.BadRequest) { }
}