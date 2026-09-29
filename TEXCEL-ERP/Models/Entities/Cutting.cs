using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents cutting operations in production.
    /// </summary>
    [Table("Cuttings")]
    public class Cutting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CuttingId { get; set; }

        [Required]
        [Display(Name = "Production Order")]
        public int ProductionOrderId { get; set; }

        [Required]
        [Display(Name = "Fabric")]
        public int FabricId { get; set; }

        [Display(Name = "Fabric Batch")]
        public int? FabricBatchId { get; set; }

        [Display(Name = "Cutting Date")]
        [DataType(DataType.Date)]
        public DateTime CuttingDate { get; set; } = DateTime.Now;

        [Display(Name = "Marker Plan")]
        [StringLength(200)]
        public string MarkerPlan { get; set; }

        [Display(Name = "Marker Efficiency (%)")]
        public decimal? MarkerEfficiency { get; set; }

        [Display(Name = "Fabric Consumed")]
        public decimal FabricConsumed { get; set; }

        [Display(Name = "Cut Pieces")]
        public int CutPieces { get; set; }

        [Display(Name = "Damaged Pieces")]
        public int DamagedPieces { get; set; } = 0;

        [Display(Name = "Wastage (%)")]
        public decimal? WastagePercentage { get; set; }

        [Display(Name = "Operator")]
        [StringLength(100)]
        public string Operator { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(500)]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("ProductionOrderId")]
        public virtual ProductionOrder ProductionOrder { get; set; }

        [ForeignKey("FabricId")]
        public virtual Fabric Fabric { get; set; }

        [ForeignKey("FabricBatchId")]
        public virtual FabricBatch FabricBatch { get; set; }
    }
}