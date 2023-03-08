using ClosedXML.Excel;
using FinanceAccounting.SearchContexts;

namespace FinanceAccounting.Interfaces;

public interface IExcelService
{
    Task<XLWorkbook> GetFile(ExpenseSearchContext searchContext, int userId);
}