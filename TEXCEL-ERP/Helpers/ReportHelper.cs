using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// Report formatting helper.
    /// </summary>
    public static class ReportHelper
    {
        public static DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

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

        public static string GetReportTitle(string reportType)
        {
            switch (reportType)
            {
                case "Employee": return "Employee Report";
                case "Attendance": return "Attendance Report";
                case "Payroll": return "Payroll Report";
                case "Inventory": return "Inventory Report";
                case "Sales": return "Sales Report";
                case "Purchase": return "Purchase Report";
                case "Production": return "Production Report";
                default: return "Report";
            }
        }
    }
}