using System.IO;
using System.Web;
using System.Web.Mvc;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// Data export helper (Excel/CSV/PDF).
    /// </summary>
    public static class ExportHelper
    {
        public static FileContentResult ExportToExcel(byte[] fileBytes, string fileName)
        {
            return new FileContentResult(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"{fileName}_{System.DateTime.Now:yyyyMMddHHmmss}.xlsx"
            };
        }

        public static FileContentResult ExportToPDF(byte[] fileBytes, string fileName)
        {
            return new FileContentResult(fileBytes, "application/pdf")
            {
                FileDownloadName = $"{fileName}_{System.DateTime.Now:yyyyMMddHHmmss}.pdf"
            };
        }

        public static FileContentResult ExportToCSV(string csvContent, string fileName)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            return new FileContentResult(bytes, "text/csv")
            {
                FileDownloadName = $"{fileName}_{System.DateTime.Now:yyyyMMddHHmmss}.csv"
            };
        }
    }
}