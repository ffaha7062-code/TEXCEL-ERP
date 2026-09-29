using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents finishing operations (Ironing, Packing, etc.).
    /// </summary>
    [Table("Finishings")]
    public class Finishing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FinishingId { get; set; }

        [Required]
        [Display(Name = "Production Order")]
        public int ProductionOrderId { get; set; }

        [Display(Name = "Finishing Date")]
        [DataType(DataType.Date)]
        public DateTime FinishingDate { get; set; } = DateTime.Now;

        [Display(Name = "Operation")]
        [StringLength(100)]
        public string Operation { get; set; } // Trimming, Ironing, Button, Label, Packing

        [Display(Name = "Operator")]
        [StringLength(100)]
        public string Operator { get; set; }

        [Display(Name = "Input Pieces")]
        public int InputPieces { get; set; }

        [Display(Name = "Output Pieces")]
        public int OutputPieces { get; set; }

        [Display(Name = "Rejected Pieces")]
        public int RejectedPieces { get; set; } = 0;

        [Display(Name = "Rework Pieces")]
        public int ReworkPieces { get; set; } = 0;

        [Display(Name = "Efficiency (%)")]
        public decimal? Efficiency { get; set; }

        [Display(Name = "Cartons Packed")]
        public int? CartonsPacked { get; set; }

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