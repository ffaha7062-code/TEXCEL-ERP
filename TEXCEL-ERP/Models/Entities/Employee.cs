using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents an employee in the organization.
    /// </summary>
    [Table("Employees")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50)]   // 20 → 50
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

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

        // =============================================
        // PERSONAL INFO
        // =============================================
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
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
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
        // CONTACT INFO
        // =============================================
        [Required]
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
        // EMPLOYMENT INFO
        // =============================================
        [Required]
        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }

        [Display(Name = "Confirmation Date")]
        [DataType(DataType.Date)]
        public DateTime? ConfirmationDate { get; set; }

        [Display(Name = "Resignation Date")]
        [DataType(DataType.Date)]
        public DateTime? ResignationDate { get; set; }

        [Display(Name = "Employee Status")]
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Probation;

        // 🆕 TEXTILE SPECIFIC
        [Display(Name = "Skill Type")]
        public SkillType? SkillType { get; set; }

        [StringLength(100)]
        [Display(Name = "Machine Assigned")]
        public string MachineAssigned { get; set; }

        [Display(Name = "Reporting Manager")]
        public int? ReportingManagerId { get; set; }

        // =============================================
        // SALARY INFO
        // =============================================
        [Display(Name = "Basic Salary")]
        [Range(0, double.MaxValue)]
        public decimal BasicSalary { get; set; }

        // 🆕 TEXTILE SPECIFIC
        [Display(Name = "Piece Rate")]
        [Range(0, double.MaxValue)]
        public decimal? PieceRate { get; set; }

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
        // DOCUMENTS
        // =============================================
        [StringLength(500)]
        [Display(Name = "Photo Path")]
        public string PhotoPath { get; set; }

        [StringLength(500)]
        [Display(Name = "CNIC Front Path")]
        public string CNICFrontPath { get; set; }

        [StringLength(500)]
        [Display(Name = "CNIC Back Path")]
        public string CNICBackPath { get; set; }

        // =============================================
        // SYSTEM
        // =============================================
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // =============================================
        // NAVIGATION PROPERTIES
        // =============================================
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("DesignationId")]
        public virtual Designation Designation { get; set; }

        [ForeignKey("ShiftId")]
        public virtual Shift Shift { get; set; }

        [ForeignKey("ReportingManagerId")]
        public virtual Employee ReportingManager { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Leave> Leaves { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
        public virtual ICollection<SalaryStructure> SalaryStructures { get; set; }
        public virtual ICollection<EmployeeDocument> Documents { get; set; }

        public Employee()
        {
            Attendances = new HashSet<Attendance>();
            Leaves = new HashSet<Leave>();
            Payrolls = new HashSet<Payroll>();
            Loans = new HashSet<Loan>();
            SalaryStructures = new HashSet<SalaryStructure>();
            Documents = new HashSet<EmployeeDocument>();
        }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}