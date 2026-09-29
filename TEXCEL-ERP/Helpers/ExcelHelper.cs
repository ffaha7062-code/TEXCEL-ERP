using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using OfficeOpenXml;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// Excel import/export helper using EPPlus.
    /// </summary>
    public static class ExcelHelper
    {
        public static DataTable ReadExcel(HttpPostedFileBase file)
        {
            var dt = new DataTable();
            using (var package = new ExcelPackage(file.InputStream))
            {
                var worksheet = package.Workbook.Worksheets[1];
                var rowCount = worksheet.Dimension.Rows;
                var colCount = worksheet.Dimension.Columns;

                for (int col = 1; col <= colCount; col++)
                {
                    dt.Columns.Add(worksheet.Cells[1, col].Value?.ToString() ?? $"Column{col}");
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    var dataRow = dt.NewRow();
                    for (int col = 1; col <= colCount; col++)
                    {
                        dataRow[col - 1] = worksheet.Cells[row, col].Value?.ToString() ?? "";
                    }
                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }

        public static byte[] ExportToExcel(DataTable data, string sheetName = "Sheet1")
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetName);
                worksheet.Cells["A1"].LoadFromDataTable(data, true);

                using (var range = worksheet.Cells[1, 1, 1, data.Columns.Count])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(108, 60, 176));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                worksheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }

        public static byte[] ExportToExcel<T>(List<T> list, string sheetName = "Sheet1")
        {
            var dt = ToDataTable(list);
            return ExportToExcel(dt, sheetName);
        }

        private static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}