using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents current stock of a fabric batch in a branch.
    /// </summary>
    [Table("FabricStocks")]
    public class FabricStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FabricStockId { get; set; }

        [Required]
        [Display(Name = "Fabric")]
        public int FabricId { get; set; }

        [Display(Name = "Fabric Batch")]
        public int? FabricBatchId { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Warehouse")]
        [StringLength(100)]
        public string Warehouse { get; set; }

        [Display(Name = "Location")]
        [StringLength(100)]
        public string Location { get; set; }

        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }

        [Display(Name = "Reserved Quantity")]
        public decimal ReservedQuantity { get; set; } = 0;

        [Display(Name = "Available Quantity")]
        public decimal AvailableQuantity => Quantity - ReservedQuantity;

        [Display(Name = "Unit of Measurement")]
        [StringLength(20)]
        public string UOM { get; set; } = "Meter";

        [Display(Name = "Rate Per Unit")]
        public decimal RatePerUnit { get; set; }

        [Display(Name = "Total Value")]
        public decimal TotalValue => Quantity * RatePerUnit;

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("FabricId")]
        public virtual Fabric Fabric { get; set; }

        [ForeignKey("FabricBatchId")]
        public virtual FabricBatch FabricBatch { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
    }
}