using ClosedXML.Extensions;
using FinanceAccounting.Controllers.Abstractions;
using FinanceAccounting.Interfaces;
using FinanceAccounting.SearchContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers;

[ApiController]
[Route("excel")]
public class ExcelController : BaseController
{
    
    ///<summary/>
    private readonly IExcelService _excelService;
    
    ///<summary/>
    public ExcelController(IExcelService excelService)
    {
        _excelService = excelService;
    }
    
    //TODO сделать все суммари
    [Authorize(Roles = "Administrator,User")]
    [HttpPost]
    //TODO сделать выбор не только даты но и времени
    public async Task<IActionResult> GetExcelFile([FromQuery]ExpenseSearchContext searchContext)
    {
        var userId = GetUserId();
        var wb = await _excelService.GetFile(searchContext, userId);
        return wb.Deliver("ExcelReport.xlsx");
    }

}