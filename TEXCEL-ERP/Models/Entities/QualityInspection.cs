using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a quality inspection record.
    /// </summary>
    [Table("QualityInspections")]
    public class QualityInspection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QualityInspectionId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Inspection Number")]
        public string InspectionNumber { get; set; }

        [Display(Name = "GRN")]
        public int? GRNId { get; set; }

        [Display(Name = "Production Order")]
        public int? ProductionOrderId { get; set; }

        // ✅ FIX: FabricId ko nullable karein
        [Display(Name = "Fabric")]
        public int? FabricId { get; set; }

        [Display(Name = "Inspection Date")]
        [DataType(DataType.Date)]
        public DateTime InspectionDate { get; set; } = DateTime.Now;

        [Display(Name = "Inspector")]
        [StringLength(100)]
        public string Inspector { get; set; }

        [Display(Name = "Inspected Quantity")]
        public decimal InspectedQuantity { get; set; }

        [Display(Name = "Passed Quantity")]
        public decimal PassedQuantity { get; set; }

        [Display(Name = "Failed Quantity")]
        public decimal FailedQuantity { get; set; }

        [Display(Name = "Rework Quantity")]
        public decimal ReworkQuantity { get; set; } = 0;

        [Display(Name = "Status")]
        public QCStatus Status { get; set; } = QCStatus.Pending;

        [Display(Name = "Defect Type")]
        [StringLength(100)]
        public string DefectType { get; set; }

        [Display(Name = "Defect Description")]
        [StringLength(500)]
        public string DefectDescription { get; set; }

        [Display(Name = "Defect Image Path")]
        [StringLength(500)]
        public string DefectImagePath { get; set; }

        [Display(Name = "Rejection (%)")]
        public decimal? RejectionPercentage { get; set; }

        [Display(Name = "Grade")]
        public FabricGrade? Grade { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(500)]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("GRNId")]
        public virtual GRN GRN { get; set; }

        [ForeignKey("ProductionOrderId")]
        public virtual ProductionOrder ProductionOrder { get; set; }

        [ForeignKey("FabricId")]
        public virtual Fabric Fabric { get; set; }
    }
}