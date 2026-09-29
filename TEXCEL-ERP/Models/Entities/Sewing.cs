using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents sewing line operations.
    /// </summary>
    [Table("Sewings")]
    public class Sewing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SewingId { get; set; }

        [Required]
        [Display(Name = "Production Order")]
        public int ProductionOrderId { get; set; }

        [Display(Name = "Sewing Date")]
        [DataType(DataType.Date)]
        public DateTime SewingDate { get; set; } = DateTime.Now;

        [Display(Name = "Line Number")]
        [StringLength(50)]
        public string LineNumber { get; set; }

        [Display(Name = "Operator")]
        [StringLength(100)]
        public string Operator { get; set; }

        [Display(Name = "Operation")]
        [StringLength(100)]
        public string Operation { get; set; }

        [Display(Name = "SMV")]
        public decimal? SMV { get; set; }

        [Display(Name = "Produced Pieces")]
        public int ProducedPieces { get; set; }

        [Display(Name = "Rejected Pieces")]
        public int RejectedPieces { get; set; } = 0;

        [Display(Name = "Efficiency (%)")]
        public decimal? Efficiency { get; set; }

        [Display(Name = "Downtime (Minutes)")]
        public int DowntimeMinutes { get; set; } = 0;

        [Display(Name = "Downtime Reason")]
        [StringLength(200)]
        public string DowntimeReason { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(500)]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("ProductionOrderId")]
        public virtual ProductionOrder ProductionOrder { get; set; }
    }
}