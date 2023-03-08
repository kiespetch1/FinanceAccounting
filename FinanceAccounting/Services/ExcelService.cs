using ClosedXML.Excel;
using FinanceAccounting.Interfaces;
using FinanceAccounting.SearchContexts;

namespace FinanceAccounting.Services;

public class ExcelService : IExcelService
{
    private readonly ApplicationContext _ctx;

    public ExcelService(ApplicationContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<XLWorkbook> GetFile(ExpenseSearchContext searchContext, int userId)
    {
        using var workbook = new XLWorkbook();
        var file = Task.Run(() =>
        {
            var wb = new XLWorkbook();
            
            var incomeSheet = wb.AddWorksheet("Income");
            incomeSheet.FirstCell().SetValue($"Income from {searchContext.From} to {searchContext.To}");
            incomeSheet.Cell("A2").SetValue("ID");
            incomeSheet.Cell("B2").SetValue("Name");
            incomeSheet.Cell("C2").SetValue("Amount");
            incomeSheet.Cell("D2").SetValue("Date");
            incomeSheet.Range("A2:D2").Style.Font.Bold = true;
            
            var incomeList = _ctx.Income
                .Where(x => x.User == userId && x.CreatedAt >= searchContext.From && x.CreatedAt <= searchContext.To)
                .OrderBy(x => x.Id)
                .Select(x => new {x.Id, x.Name, x.Amount, x.CreatedAt});
            var incomeStartingPoint = incomeSheet.Cell("A3").InsertData(incomeList);

            var tableRange = incomeSheet.Range($"A2:D{incomeList.Count() + 2}");

            tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            incomeStartingPoint.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            
            incomeSheet.Column(1).Width = 20;
            incomeSheet.Columns().AdjustToContents(2, incomeList.Count() + 2);
            
            var expenseSheet = wb.AddWorksheet("Expenses");
            expenseSheet.FirstCell().SetValue($"Expenses from {searchContext.From} to {searchContext.To}");
            expenseSheet.Cell("A2").SetValue("ID");
            expenseSheet.Cell("B2").SetValue("Name");
            expenseSheet.Cell("C2").SetValue("Amount");
            expenseSheet.Cell("D2").SetValue("Date");
            expenseSheet.Range("A2:D2").Style.Font.Bold = true;
            
            var expensesList = _ctx.Expense
                .Where(x => x.User == userId && x.CreatedAt >= searchContext.From && x.CreatedAt <= searchContext.To)
                .OrderBy(x => x.Id)
                .Select(x => new {x.Id, x.Name, x.Amount, x.CreatedAt});
            var expenseStartingPoint = expenseSheet.Cell("A3").InsertData(expensesList);

            var expenseTableRange = expenseSheet.Range($"A2:D{expensesList.Count() + 2}");

            expenseTableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            expenseTableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            expenseStartingPoint.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            
            expenseSheet.Column(1).Width = 20;
            expenseSheet.Columns().AdjustToContents(2, expensesList.Count() + 2);
            return wb;
        });
        return await file;
    }
}