using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for user registration.
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }
    }
}