using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a designation within a department.
    /// </summary>
    [Table("Designations")]
    public class Designation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesignationId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Designation Code")]
        public string DesignationCode { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Designation Name")]
        public string DesignationName { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Grade")]
        [StringLength(10)]
        public string Grade { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public Designation()
        {
            Employees = new HashSet<Employee>();
        }
    }
}