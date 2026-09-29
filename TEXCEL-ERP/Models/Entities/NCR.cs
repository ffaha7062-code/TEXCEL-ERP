using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a Non-Conformance Report (NCR).
    /// </summary>
    [Table("NCRs")]
    public class NCR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NCRId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "NCR Number")]
        public string NCRNumber { get; set; }

        [Display(Name = "Quality Inspection")]
        public int? QualityInspectionId { get; set; }

        [Display(Name = "NCR Date")]
        [DataType(DataType.Date)]
        public DateTime NCRDate { get; set; } = DateTime.Now;

        [Display(Name = "Issue Description")]
        [StringLength(1000)]
        public string IssueDescription { get; set; }

        [Display(Name = "Root Cause")]
        [StringLength(1000)]
        public string RootCause { get; set; }

        [Display(Name = "Corrective Action")]
        [StringLength(1000)]
        public string CorrectiveAction { get; set; }

        [Display(Name = "Preventive Action")]
        [StringLength(1000)]
        public string PreventiveAction { get; set; }

        [Display(Name = "Responsible Person")]
        [StringLength(100)]
        public string ResponsiblePerson { get; set; }

        [Display(Name = "Target Date")]
        [DataType(DataType.Date)]
        public DateTime? TargetDate { get; set; }

        [Display(Name = "Closure Date")]
        [DataType(DataType.Date)]
        public DateTime? ClosureDate { get; set; }

        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Open"; // Open, InProgress, Closed

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("QualityInspectionId")]
        public virtual QualityInspection QualityInspection { get; set; }

        public virtual ICollection<CAPA> CAPAs { get; set; }

        public NCR()
        {
            CAPAs = new HashSet<CAPA>();
        }
    }
}