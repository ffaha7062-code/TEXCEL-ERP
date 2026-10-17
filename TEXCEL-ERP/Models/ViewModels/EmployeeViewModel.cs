using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Employee CRUD and filtering.
    /// </summary>
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee Code is required")]
        [StringLength(20)]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [Display(Name = "Designation")]
        public int DesignationId { get; set; }

        [Display(Name = "Shift")]
        public int? ShiftId { get; set; }

        // =============================================
        // PERSONAL
        // =============================================
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(100)]
        [Display(Name = "Father/Husband Name")]
        public string FatherHusbandName { get; set; }

        [Required(ErrorMessage = "CNIC is required")]
        [StringLength(20)]
        [Display(Name = "CNIC")]
        public string CNIC { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [StringLength(50)]
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [StringLength(50)]
        [Display(Name = "Religion")]
        public string Religion { get; set; }

        [StringLength(20)]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        [StringLength(20)]
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        // =============================================
        // CONTACT
        // =============================================
        [Required(ErrorMessage = "Mobile Number is required")]
        [StringLength(20)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [StringLength(20)]
        [Display(Name = "Emergency Contact")]
        public string EmergencyContact { get; set; }

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

        // =============================================
        // EMPLOYMENT
        // =============================================
        [Required(ErrorMessage = "Joining Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Confirmation Date")]
        public DateTime? ConfirmationDate { get; set; }

        [Display(Name = "Employee Status")]
        public EmployeeStatus Status { get; set; }

        [Display(Name = "Skill Type")]
        public SkillType? SkillType { get; set; }

        [StringLength(100)]
        [Display(Name = "Machine Assigned")]
        public string MachineAssigned { get; set; }

        [Display(Name = "Reporting Manager")]
        public int? ReportingManagerId { get; set; }

        // =============================================
        // SALARY
        // =============================================
        [Required(ErrorMessage = "Basic Salary is required")]
        [Range(0, double.MaxValue)]
        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Piece Rate")]
        public decimal? PieceRate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        [Display(Name = "Bank Account Number")]
        public string BankAccountNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [StringLength(100)]
        [Display(Name = "Bank Branch Name")]
        public string BankBranchName { get; set; }

        // =============================================
        // DOCUMENTS (File Upload)
        // =============================================
        [Display(Name = "Photo Path")]
        public string PhotoPath { get; set; }

        [Display(Name = "CNIC Front Path")]
        public string CNICFrontPath { get; set; }

        [Display(Name = "CNIC Back Path")]
        public string CNICBackPath { get; set; }

        [Display(Name = "Upload Photo")]
        public HttpPostedFileBase PhotoFile { get; set; }

        [Display(Name = "Upload CNIC Front")]
        public HttpPostedFileBase CNICFrontFile { get; set; }

        [Display(Name = "Upload CNIC Back")]
        public HttpPostedFileBase CNICBackFile { get; set; }

        // =============================================
        // DROPDOWNS
        // =============================================
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public List<Designation> Designations { get; set; }
        public List<Shift> Shifts { get; set; }
        public List<Employee> Managers { get; set; }

        // =============================================
        // FILTERS
        // =============================================
        public string SearchTerm { get; set; }
        public int? FilterBranchId { get; set; }
        public int? FilterDepartmentId { get; set; }
        public int? FilterDesignationId { get; set; }
        public int? FilterShiftId { get; set; }
        public EmployeeStatus? FilterStatus { get; set; }
        public Gender? FilterGender { get; set; }
        public string FilterBloodGroup { get; set; }
        public SkillType? FilterSkillType { get; set; }
        public string FilterMachine { get; set; }
        public DateTime? FilterJoiningFrom { get; set; }
        public DateTime? FilterJoiningTo { get; set; }
        public decimal? FilterSalaryFrom { get; set; }
        public decimal? FilterSalaryTo { get; set; }
    }
}