using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for User CRUD and filtering.
    /// </summary>
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
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

        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Last Login")]
        public DateTime? LastLogin { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        // For Dropdowns
        public List<Branch> Branches { get; set; }
        public List<string> Roles { get; set; }

        // For Filters
        public string SearchTerm { get; set; }
        public int? FilterBranchId { get; set; }
        public string FilterRole { get; set; }
        public bool? FilterIsActive { get; set; }
    }
}