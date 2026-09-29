using System;
using System.Security.Cryptography;
using System.Text;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// Common utility helper methods.
    /// </summary>
    public static class CommonHelper
    {
        public static string GenerateEmployeeCode(string branchCode, int sequence)
        {
            return $"EMP-{branchCode}-{DateTime.Now.Year}-{sequence:D4}";
        }

        public static string GeneratePONumber(int sequence)
        {
            return $"PO-{DateTime.Now.Year}-{sequence:D4}";
        }

        public static string GenerateSONumber(int sequence)
        {
            return $"SO-{DateTime.Now.Year}-{sequence:D4}";
        }

        public static string GenerateInvoiceNumber(int sequence)
        {
            return $"INV-{DateTime.Now.Year}-{sequence:D4}";
        }

        public static string GenerateGRNNumber(int sequence)
        {
            return $"GRN-{DateTime.Now.Year}-{sequence:D4}";
        }

        public static string GenerateVoucherNumber(string type, int sequence)
        {
            return $"{type}-{DateTime.Now.Year}-{sequence:D4}";
        }

        public static string GetMonthName(int month)
        {
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString("N2");
        }

        public static string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd-MMM-yyyy") : "";
        }

        public static string GetFileExtension(string fileName)
        {
            return System.IO.Path.GetExtension(fileName).ToLower();
        }

        public static bool IsValidFileExtension(string fileName, string[] allowedExtensions)
        {
            var ext = GetFileExtension(fileName);
            return Array.Exists(allowedExtensions, e => e.Equals(ext, StringComparison.OrdinalIgnoreCase));
        }

        public static string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%";
            var res = new StringBuilder();
            using (var rng = new RNGCryptoServiceProvider())
            {
                var uintBuffer = new byte[sizeof(uint)];
                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    var num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(validChars[(int)(num % (uint)validChars.Length)]);
                }
            }
            return res.ToString();
        }

        public static int GetFiscalYear()
        {
            return DateTime.Now.Month >= 7 ? DateTime.Now.Year : DateTime.Now.Year - 1;
        }
    }
}