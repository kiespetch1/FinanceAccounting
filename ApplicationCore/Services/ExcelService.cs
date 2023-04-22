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
    /// <inheritdoc cref="IExcelService.GetFile(int, CashflowSearchContext)"/>
    public async Task<XLWorkbook> GetFile(int userId, CashflowSearchContext searchContext)
    {
        using var workbook = new XLWorkbook();
        var wb = new XLWorkbook();

        var incomeSheet = wb.AddWorksheet("Income")
            .SetFirstCellValue($"Income from {searchContext.From} to {searchContext.To}")
            .AddHeader();

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
            
        var expenseSheet = wb.AddWorksheet("Expenses")
            .SetFirstCellValue($"Expenses from {searchContext.From} to {searchContext.To}")
            .AddHeader();

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
        
        return await Task.FromResult(wb);
    }

    /// <inheritdoc cref="IExcelService.ApplyChangesFromXlsx(int, IFormFile)"/>
    public async Task ApplyChangesFromXlsx(int userId, IFormFile report)
    {
        await using var reportPath = report.OpenReadStream();

        var wb = new XLWorkbook(reportPath);
        var incomeSheet = wb.Worksheet("Income");
        var incomeLastRowNumber = incomeSheet.LastCellUsed().Address.RowNumber;

        for (var incomeRowNumber = 3; incomeRowNumber <= incomeLastRowNumber; incomeRowNumber++)
        {
            var idCell = Convert.ToInt32(incomeSheet.Cell($"A{incomeRowNumber}").GetString());
            var nameCell = incomeSheet.Cell($"B{incomeRowNumber}").GetString();
            var amountCell = Convert.ToDecimal(incomeSheet.Cell($"C{incomeRowNumber}").GetString());
            var dateCell = incomeSheet.Cell($"D{incomeRowNumber}").GetDateTime();
                
            var currentRecord = _ctx.Income.SingleOrDefault(x => x.Id == idCell);
            if (currentRecord.User == userId)
            {
                currentRecord.Name = nameCell;
                currentRecord.Amount = amountCell;
                currentRecord.CreatedAt = dateCell;
                currentRecord.UpdatedAt = DateTime.Now;
            }
        }
        
        var expenseSheet = wb.Worksheet("Expenses");
        
        var expenseLastRowNumber = expenseSheet.LastCellUsed().Address.RowNumber;
        
        for (var expenseRowNumber = 3; expenseRowNumber <= expenseLastRowNumber; expenseRowNumber++)
        {
            var idCell = Convert.ToInt32(expenseSheet.Cell($"A{expenseRowNumber}").GetString());
            var nameCell = expenseSheet.Cell($"B{expenseRowNumber}").GetString();
            var amountCell = Convert.ToDecimal(expenseSheet.Cell($"C{expenseRowNumber}").GetString());
            var dateCell = expenseSheet.Cell($"D{expenseRowNumber}").GetDateTime();
                
            var currentRecord = _ctx.Expense.SingleOrDefault(x => x.Id == idCell);
            if (currentRecord.User == userId)
            {
                currentRecord.Name = nameCell;
                currentRecord.Amount = amountCell;
                currentRecord.CreatedAt = dateCell;
                currentRecord.UpdatedAt = DateTime.Now;
            }
        }
        await _ctx.SaveChangesAsync();
    }
}