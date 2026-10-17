using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Helpers;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Identity;
using TEXCEL_ERP.Models.ViewModels;
using TEXCEL_ERP.Services;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// Account controller for authentication and user management.
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IEmailService _emailService;

        public AccountController()
        {
            _emailService = new EmailService();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IEmailService emailService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _emailService = emailService;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // =============================================
        // LOGIN — GET
        // =============================================
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // ✅ Dono check karein
            if (User.Identity.IsAuthenticated && SessionHelper.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            // ✅ Agar Identity cookie hai lekin SessionHelper NULL hai
            // Toh session clear karein taake fresh login ho
            if (User.Identity.IsAuthenticated && !SessionHelper.IsAuthenticated)
            {
                // Session clear karo — fresh start
                try
                {
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                }
                catch { }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // =============================================
        // LOGIN — POST
        // =============================================
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        var user = await UserManager.FindByEmailAsync(model.Email);
                        if (user != null)
                        {
                            user.LastLogin = DateTime.Now;
                            await UserManager.UpdateAsync(user);

                            var roles = await UserManager.GetRolesAsync(user.Id);
                            var primaryRole = roles.FirstOrDefault() ?? "Employee";

                            string branchName = null;
                            if (user.BranchId.HasValue)
                            {
                                using (var context = new ApplicationDbContext())
                                {
                                    var branch = context.Branches.Find(user.BranchId.Value);
                                    if (branch != null)
                                    {
                                        branchName = branch.BranchName;
                                    }
                                }
                            }

                            SessionHelper.UserId = user.Id;
                            SessionHelper.UserName = user.Email;
                            SessionHelper.FullName = user.FullName;
                            SessionHelper.Role = primaryRole;
                            SessionHelper.BranchId = user.BranchId;
                            SessionHelper.BranchName = branchName;
                            SessionHelper.EmployeeId = user.EmployeeId;

                            HttpContext.Session["LastActivity"] = DateTime.Now;
                        }

                        return RedirectToLocal(returnUrl);

                    case SignInStatus.LockedOut:
                        ModelState.AddModelError("", "Account locked. Please try again after 5 minutes.");
                        return View(model);

                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid email or password.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                // ✅ TEMPORARY: Asli error screen pe dikhao (debugging ke liye)
                System.Diagnostics.Debug.WriteLine("LOGIN ERROR: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("STACK: " + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("INNER: " + ex.InnerException.Message);
                }

                ModelState.AddModelError("", "DEBUG: " + ex.Message);
                return View(model);
            }
        }

        [AllowAnonymous]
        public JsonResult CheckSession()
        {
            return Json(new
            {
                UserId = SessionHelper.UserId,
                Role = SessionHelper.Role,
                BranchId = SessionHelper.BranchId,
                EmployeeId = SessionHelper.EmployeeId,
                IsAuthenticated = SessionHelper.IsAuthenticated
            }, JsonRequestBehavior.AllowGet);
        }

        // =============================================
        // REGISTER — GET
        // =============================================
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        // =============================================
        // REGISTER — POST
        // =============================================
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    EmailConfirmed = true
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Employee");
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Dashboard");
                }

                AddErrors(result);
            }

            return View(model);
        }

        // =============================================
        // LOG OFF
        // =============================================
        public ActionResult LogOff()
        {
            // OWIN Authentication Sign Out — ASP.NET Identity ke liye ZAROORI
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // Session clear karo
            SessionHelper.Clear();
            Session.Clear();
            Session.Abandon();

            // Login pe redirect — ReturnUrl NAHI
            return RedirectToAction("Login", "Account");
        }

        // =============================================
        // FORGOT PASSWORD
        // =============================================
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                _emailService.SendForgotPasswordEmail(model.Email, callbackUrl);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // =============================================
        // RESET PASSWORD
        // =============================================
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // =============================================
        // CHANGE PASSWORD
        // =============================================
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                ViewBag.StatusMessage = "Your password has been changed successfully.";
                return View();
            }

            AddErrors(result);
            return View(model);
        }

        // =============================================
        // HELPERS
        // =============================================
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            // ✅ Block dangerous/loop-creating URLs
            if (!string.IsNullOrEmpty(returnUrl)
                && Url.IsLocalUrl(returnUrl)
                && !returnUrl.Contains("/Account/LogOff")
                && !returnUrl.Contains("/Account/Login")
                && !returnUrl.Contains("/Account/Register")
                && !returnUrl.Contains("/Account/Logout"))
            {
                return Redirect(returnUrl);
            }

            // Default: Dashboard
            return RedirectToAction("Index", "Dashboard");
        }
    }
}