using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Web;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Models.Identity
{
    /// <summary>
    /// Application SignIn Manager for login operations.
    /// </summary>
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(
                context.GetUserManager<ApplicationUserManager>(),
                context.Authentication);
        }
    }
}