using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// Database backup helper.
    /// </summary>
    public static class BackupHelper
    {
        public static bool BackupDatabase(string backupPath = null)
        {
            try
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var builder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = builder.InitialCatalog;

                if (string.IsNullOrEmpty(backupPath))
                {
                    backupPath = HttpContext.Current.Server.MapPath($"~/App_Data/Backups/{databaseName}_{DateTime.Now:yyyyMMddHHmmss}.bak");
                }

                var directory = Path.GetDirectoryName(backupPath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var query = $"BACKUP DATABASE [{databaseName}] TO DISK = '{backupPath}' WITH FORMAT, INIT";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 300;
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}