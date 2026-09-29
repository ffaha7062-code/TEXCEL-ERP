using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a department within a branch.
    /// </summary>
    [Table("Departments")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Designation> Designations { get; set; }

        public Department()
        {
            Employees = new HashSet<Employee>();
            Designations = new HashSet<Designation>();
        }
    }
}