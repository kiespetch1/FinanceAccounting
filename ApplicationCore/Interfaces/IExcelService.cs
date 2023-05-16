using ApplicationCore.Models.SearchContexts;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;

namespace ApplicationCore.Interfaces;

public interface IExcelService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId">Current user ID.</param>
    /// <param name="searchContext">Specified period of time.</param>
    /// <returns></returns>
    Task<XLWorkbook> GetFile(int userId, CashflowSearchContext searchContext);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="report">Excel report file.</param>
    /// <param name="userId">Current user ID.</param>
    /// <returns></returns>
    Task ApplyChangesFromXlsx(int userId, IFormFile report);
}