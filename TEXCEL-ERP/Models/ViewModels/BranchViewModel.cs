using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Branch CRUD and filtering.
    /// </summary>
    public class BranchViewModel
    {
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Company is required")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Branch Code is required")]
        [StringLength(20)]
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Required(ErrorMessage = "Branch Name is required")]
        [StringLength(200)]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [StringLength(500)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [StringLength(100)]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(20)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100)]
        [Display(Name = "Manager Name")]
        public string ManagerName { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        // Stats
        [Display(Name = "Total Employees")]
        public int TotalEmployees { get; set; }

        [Display(Name = "Total Departments")]
        public int TotalDepartments { get; set; }

        // For Dropdown
        public List<Company> Companies { get; set; }

        // For Filters
        public string SearchTerm { get; set; }
        public bool? FilterIsActive { get; set; }
        public int? FilterCompanyId { get; set; }
    }
}