using ClosedXML.Excel;

namespace ApplicationCore.Extensions;

public static class ExcelExtension
{
    /// <summary>
    /// Adds a header to report sheet.
    /// </summary>
    /// <param name="ws">Worksheet to add header.</param>
    /// <returns></returns>
    public static IXLWorksheet AddHeader(this IXLWorksheet ws)
    {
        ws.Cell("A2").SetValue("ID");
        ws.Cell("B2").SetValue("Name");
        ws.Cell("C2").SetValue("Amount");
        ws.Cell("D2").SetValue("Date");
        ws.Range("A2:D2").Style.Font.Bold = true;
        
        return ws;
    }
    /// <summary>
    /// Sets the value of first cell.
    /// </summary>
    /// <param name="ws">Worksheet to set first cell value.</param>
    /// <param name="value">Value to set in cell.</param>
    /// <returns></returns>
    public static IXLWorksheet SetFirstCellValue(this IXLWorksheet ws, string value)
    {
        ws.FirstCell().SetValue(value);
        
        return ws;
    }
}