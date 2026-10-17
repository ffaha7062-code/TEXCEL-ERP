using System.Web.Mvc;
using TEXCEL_ERP.Filters;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// Home controller — redirects to dashboard.
    /// </summary>
    [BranchAuthorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        /// <summary>
        /// Unauthorized page — AllowAnonymous zaroori hai,
        /// warna session expire hone pe redirect loop ban jayega.
        /// </summary>
        [AllowAnonymous]
        public ActionResult Unauthorized()
        {
            return View();
        }

        /// <summary>
        /// Error page — AllowAnonymous zaroori hai.
        /// </summary>
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }
    }
}