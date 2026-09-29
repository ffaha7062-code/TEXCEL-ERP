using System;
using System.IO;
using System.Web;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// File upload/download helper.
    /// </summary>
    public static class FileHelper
    {
        public static string UploadFile(HttpPostedFileBase file, string folderPath, string prefix = "")
        {
            if (file == null || file.ContentLength == 0) return null;

            var fileName = $"{prefix}_{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileName(file.FileName)}";
            var directory = HttpContext.Current.Server.MapPath($"~/Uploads/{folderPath}");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var fullPath = Path.Combine(directory, fileName);
            file.SaveAs(fullPath);

            return $"/Uploads/{folderPath}/{fileName}";
        }

        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath)) return false;

                var fullPath = HttpContext.Current.Server.MapPath($"~{filePath}");
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string GetFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        public static bool IsImageFile(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();
            return ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".bmp";
        }

        public static bool IsDocumentFile(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();
            return ext == ".pdf" || ext == ".doc" || ext == ".docx" || ext == ".xls" || ext == ".xlsx" || ext == ".txt";
        }
    }
}