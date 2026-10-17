using System;
using System.Web;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// Session management helper.
    /// Provides easy access to logged-in user information.
    /// </summary>
    public static class SessionHelper
    {
        // =============================================
        // SAFE SESSION ACCESS
        // =============================================
        private static bool IsSessionAvailable
        {
            get
            {
                return HttpContext.Current != null
                    && HttpContext.Current.Session != null;
            }
        }

        // =============================================
        // USER INFO
        // =============================================
        public static string UserId
        {
            get
            {
                if (!IsSessionAvailable) return null;
                return HttpContext.Current.Session["UserId"] as string;
            }
            set
            {
                if (!IsSessionAvailable) return;
                HttpContext.Current.Session["UserId"] = value;
            }
        }

        public static string UserName
        {
            get
            {
                if (!IsSessionAvailable) return null;
                return HttpContext.Current.Session["UserName"] as string;
            }
            set
            {
                if (!IsSessionAvailable) return;
                HttpContext.Current.Session["UserName"] = value;
            }
        }

        public static string FullName
        {
            get
            {
                if (!IsSessionAvailable) return null;
                return HttpContext.Current.Session["FullName"] as string;
            }
            set
            {
                if (!IsSessionAvailable) return;
                HttpContext.Current.Session["FullName"] = value;
            }
        }

        // =============================================
        // ROLE
        // =============================================
        public static string Role
        {
            get
            {
                if (!IsSessionAvailable) return null;
                return HttpContext.Current.Session["Role"] as string;
            }
            set
            {
                if (!IsSessionAvailable) return;
                HttpContext.Current.Session["Role"] = value;
            }
        }

        // =============================================
        // BRANCH INFO
        // =============================================
        public static int? BranchId
        {
            get
            {
                if (!IsSessionAvailable) return null;
                var value = HttpContext.Current.Session["BranchId"];
                if (value == null) return null;
                if (value is int) return (int)value;
                int result;
                if (int.TryParse(value.ToString(), out result)) return result;
                return null;
            }
            set
            {
                if (!IsSessionAvailable) return;
                HttpContext.Current.Session["BranchId"] = value;
            }
        }

        public static string BranchName
        {
            get
            {
                if (!IsSessionAvailable) return null;
                return HttpContext.Current.Session["BranchName"] as string;
            }
            set
            {
                if (!IsSessionAvailable) return;
                HttpContext.Current.Session["BranchName"] = value;
            }
        }

        // =============================================
        // EMPLOYEE INFO
        // =============================================
        public static int? EmployeeId
        {
            get
            {
                if (!IsSessionAvailable) return null;
                var value = HttpContext.Current.Session["EmployeeId"];
                if (value == null) return null;
                if (value is int) return (int)value;
                int result;
                if (int.TryParse(value.ToString(), out result)) return result;
                return null;
            }
            set
            {
                if (!IsSessionAvailable) return;
                HttpContext.Current.Session["EmployeeId"] = value;
            }
        }

        // =============================================
        // ROLE CHECKS
        // =============================================
        public static bool IsSuperAdmin
        {
            get { return Role == "SuperAdmin"; }
        }

        public static bool IsBranchAdmin
        {
            get { return Role == "BranchAdmin"; }
        }

        public static bool IsManager
        {
            get { return Role == "Manager"; }
        }

        public static bool IsEmployee
        {
            get { return Role == "Employee"; }
        }

        public static bool IsAdmin
        {
            get { return IsSuperAdmin || IsBranchAdmin; }
        }

        public static bool IsAuthenticated
        {
            get
            {
                if (!IsSessionAvailable) return false;
                return HttpContext.Current.Session["UserId"] != null;
            }
        }

        // =============================================
        // PERMISSIONS
        // =============================================
        public static bool CanEdit
        {
            get { return IsSuperAdmin || IsBranchAdmin || IsManager; }
        }

        public static bool CanDelete
        {
            get { return IsSuperAdmin || IsBranchAdmin; }
        }

        public static bool CanViewAllBranches
        {
            get { return IsSuperAdmin; }
        }

        public static bool CanManageUsers
        {
            get { return IsSuperAdmin; }
        }

        public static bool CanManageBranches
        {
            get { return IsSuperAdmin; }
        }

        public static bool CanManageCompany
        {
            get { return IsSuperAdmin; }
        }

        // =============================================
        // SESSION MANAGEMENT
        // =============================================
        public static void Clear()
        {
            if (!IsSessionAvailable) return;
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public static DateTime? LastActivity
        {
            get
            {
                if (!IsSessionAvailable) return null;
                var value = HttpContext.Current.Session["LastActivity"];
                if (value == null) return null;
                if (value is DateTime) return (DateTime)value;
                return null;
            }
            set
            {
                if (!IsSessionAvailable) return;
                HttpContext.Current.Session["LastActivity"] = value;
            }
        }
    }
}