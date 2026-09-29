using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for employee CRUD and filtering.
    /// </summary>
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public int DesignationId { get; set; }

        [Display(Name = "Shift")]
        public int? ShiftId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(100)]
        [Display(Name = "Father/Husband Name")]
        public string FatherHusbandName { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "CNIC")]
        public string CNIC { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(500)]
        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }

        [StringLength(500)]
        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        [Display(Name = "Employee Status")]
        public EmployeeStatus Status { get; set; }

        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; }

        // For Dropdowns
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public List<Designation> Designations { get; set; }
        public List<Shift> Shifts { get; set; }

        // For Filters
        public string SearchTerm { get; set; }
        public int? FilterBranchId { get; set; }
        public int? FilterDepartmentId { get; set; }
        public int? FilterDesignationId { get; set; }
        public EmployeeStatus? FilterStatus { get; set; }
        public DateTime? FilterFromDate { get; set; }
        public DateTime? FilterToDate { get; set; }
    }
}