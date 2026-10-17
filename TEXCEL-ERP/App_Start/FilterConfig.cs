using System.Web;
using System.Web.Mvc;
using TEXCEL_ERP.Filters;

namespace TEXCEL_ERP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionLogAttribute());
            //filters.Add(new SessionTimeoutAttribute());
        }
    }
}
