using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents Corrective and Preventive Action (CAPA).
    /// </summary>
    [Table("CAPAs")]
    public class CAPA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CAPAId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "CAPA Number")]
        public string CAPANumber { get; set; }

        [Display(Name = "NCR")]
        public int? NCRId { get; set; }

        [Display(Name = "CAPA Date")]
        [DataType(DataType.Date)]
        public DateTime CAPADate { get; set; } = DateTime.Now;

        [Display(Name = "Action Type")]
        [StringLength(50)]
        public string ActionType { get; set; } // Corrective, Preventive

        [Display(Name = "Action Description")]
        [StringLength(1000)]
        public string ActionDescription { get; set; }

        [Display(Name = "Responsible Person")]
        [StringLength(100)]
        public string ResponsiblePerson { get; set; }

        [Display(Name = "Target Date")]
        [DataType(DataType.Date)]
        public DateTime? TargetDate { get; set; }

        [Display(Name = "Completion Date")]
        [DataType(DataType.Date)]
        public DateTime? CompletionDate { get; set; }

        [Display(Name = "Verification")]
        [StringLength(500)]
        public string Verification { get; set; }

        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Open"; // Open, InProgress, Completed, Verified

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("NCRId")]
        public virtual NCR NCR { get; set; }
    }
}