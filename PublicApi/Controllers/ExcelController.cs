using ClosedXML.Extensions;
using Entities.SearchContexts;
using FinanceAccounting.Controllers.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Interfaces;

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
    
    /// <summary>
    /// Returns income and expenses Excel report of the current user.
    /// </summary>
    /// <param name="searchContext">Specified period of time.</param>
    /// <returns>Excel file with income and expenses for specified period of time/ </returns>
    [Authorize(Roles = "Administrator,User")]
    [HttpPost]
    public async Task<IActionResult> GetExcelFile([FromQuery]CashflowSearchContext searchContext)
    {
        var userId = GetUserId();
        var wb = await _excelService.GetFile(userId, searchContext);
        
        return wb.Deliver("ExcelReport.xlsx");
    }
    
    /// <summary>
    /// Changes the data in the database according to the loaded Excel report file.
    /// </summary>
    /// <param name="reportFile">Excel report file with changes.</param>
    [Authorize(Roles = "Administrator,User")]
    [HttpPut]
    public async Task<ActionResult>ApplyChangesFromXlsx(IFormFile reportFile)
    {
        var userId = GetUserId();
        await _excelService.ApplyChangesFromXlsx(userId, reportFile);
        
        return Ok();
    }

}