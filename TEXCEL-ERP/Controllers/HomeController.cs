using System.Web.Mvc;
using TEXCEL_ERP.Filters;
using TEXCEL_ERP.Services;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// Home controller — redirects to dashboard.
    /// </summary>
    [SessionTimeout]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}