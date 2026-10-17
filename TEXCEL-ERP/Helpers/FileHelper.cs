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
        /// <summary>
        /// Upload file to a specific folder.
        /// </summary>
        public static string UploadFile(HttpPostedFileBase file, string folderPath, string prefix = "")
        {
            if (file == null || file.ContentLength == 0) return null;

            var extension = Path.GetExtension(file.FileName).ToLower();
            var fileName = $"{prefix}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString().Substring(0, 6)}{extension}";
            var directory = HttpContext.Current.Server.MapPath($"~/Uploads/{folderPath}");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var fullPath = Path.Combine(directory, fileName);
            file.SaveAs(fullPath);

            return $"/Uploads/{folderPath}/{fileName}";
        }

        /// <summary>
        /// Delete file from server.
        /// </summary>
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

        /// <summary>
        /// Check if file is valid image.
        /// </summary>
        public static bool IsValidImage(HttpPostedFileBase file)
        {
            if (file == null) return false;

            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return Array.IndexOf(allowed, extension) >= 0;
        }
    }
}