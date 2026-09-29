using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for forgot password.
    /// </summary>
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}