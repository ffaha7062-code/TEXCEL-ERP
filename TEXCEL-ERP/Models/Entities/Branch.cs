using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a branch of a company.
    /// </summary>
    [Table("Branches")]
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Required]
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
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<FabricStock> FabricStocks { get; set; }

        public Branch()
        {
            Employees = new HashSet<Employee>();
            Departments = new HashSet<Department>();
            FabricStocks = new HashSet<FabricStock>();
        }
    }
}