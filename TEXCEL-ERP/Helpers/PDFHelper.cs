using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Printing;
using System.IO;
using System.Xml.Linq;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// PDF generation helper using iTextSharp.
    /// </summary>
    public static class PDFHelper
    {
        public static byte[] GeneratePayslipPDF(string employeeName, string month, decimal basicSalary,
            decimal totalEarnings, decimal totalDeductions, decimal netSalary)
        {
            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Header
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, new BaseColor(108, 60, 176));
                var title = new Paragraph("TEXCEL ERP™", titleFont) { Alignment = Element.ALIGN_CENTER };
                document.Add(title);

                var subTitle = new Paragraph("Payslip - " + month, FontFactory.GetFont(FontFactory.HELVETICA, 14))
                { Alignment = Element.ALIGN_CENTER };
                document.Add(subTitle);

                document.Add(new Paragraph("\n"));

                // Employee Info Table
                var infoTable = new PdfPTable(2) { WidthPercentage = 100 };
                infoTable.AddCell(new PdfPCell(new Phrase("Employee Name")) { BackgroundColor = new BaseColor(240, 240, 240) });
                infoTable.AddCell(employeeName);
                infoTable.AddCell(new PdfPCell(new Phrase("Month")) { BackgroundColor = new BaseColor(240, 240, 240) });
                infoTable.AddCell(month);
                document.Add(infoTable);

                document.Add(new Paragraph("\n"));

                // Salary Details
                var salaryTable = new PdfPTable(2) { WidthPercentage = 100 };
                salaryTable.AddCell(new PdfPCell(new Phrase("Basic Salary")) { BackgroundColor = new BaseColor(240, 240, 240) });
                salaryTable.AddCell(basicSalary.ToString("N2"));
                salaryTable.AddCell(new PdfPCell(new Phrase("Total Earnings")) { BackgroundColor = new BaseColor(240, 240, 240) });
                salaryTable.AddCell(totalEarnings.ToString("N2"));
                salaryTable.AddCell(new PdfPCell(new Phrase("Total Deductions")) { BackgroundColor = new BaseColor(240, 240, 240) });
                salaryTable.AddCell(totalDeductions.ToString("N2"));

                var netCell = new PdfPCell(new Phrase("Net Salary", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
                { BackgroundColor = new BaseColor(108, 60, 176) };
                salaryTable.AddCell(netCell);
                salaryTable.AddCell(new PdfPCell(new Phrase(netSalary.ToString("N2"), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
                { BackgroundColor = new BaseColor(240, 240, 240) });

                document.Add(salaryTable);

                document.Add(new Paragraph("\n\n"));
                document.Add(new Paragraph("This is a computer-generated payslip.", FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10)));

                document.Close();
                return ms.ToArray();
            }
        }
    }
}