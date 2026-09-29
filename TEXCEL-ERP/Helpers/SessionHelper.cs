using System.Web;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// Session management helper.
    /// </summary>
    public static class SessionHelper
    {
        public static int? UserId
        {
            get { return HttpContext.Current.Session["UserId"] as int?; }
            set { HttpContext.Current.Session["UserId"] = value; }
        }

        public static string UserName
        {
            get { return HttpContext.Current.Session["UserName"] as string; }
            set { HttpContext.Current.Session["UserName"] = value; }
        }

        public static int? BranchId
        {
            get { return HttpContext.Current.Session["BranchId"] as int?; }
            set { HttpContext.Current.Session["BranchId"] = value; }
        }

        public static string BranchName
        {
            get { return HttpContext.Current.Session["BranchName"] as string; }
            set { HttpContext.Current.Session["BranchName"] = value; }
        }

        public static string Role
        {
            get { return HttpContext.Current.Session["Role"] as string; }
            set { HttpContext.Current.Session["Role"] = value; }
        }

        public static bool IsSuperAdmin
        {
            get { return Role == "SuperAdmin"; }
        }

        public static bool IsBranchUser
        {
            get { return Role == "BranchUser" || Role == "BranchAdmin"; }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}