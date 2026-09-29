using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a line item in a purchase order.
    /// </summary>
    [Table("PurchaseOrderDetails")]
    public class PurchaseOrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderDetailId { get; set; }

        [Required]
        [Display(Name = "Purchase Order")]
        public int PurchaseOrderId { get; set; }

        [Required]
        [Display(Name = "Fabric")]
        public int FabricId { get; set; }

        [Display(Name = "Fabric Batch")]
        public int? FabricBatchId { get; set; }

        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }

        [Display(Name = "Unit of Measurement")]
        [StringLength(20)]
        public string UOM { get; set; } = "Meter";

        [Display(Name = "Rate Per Unit")]
        public decimal RatePerUnit { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount => Quantity * RatePerUnit;

        [Display(Name = "Received Quantity")]
        public decimal ReceivedQuantity { get; set; } = 0;

        [Display(Name = "Pending Quantity")]
        public decimal PendingQuantity => Quantity - ReceivedQuantity;

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        [ForeignKey("FabricId")]
        public virtual Fabric Fabric { get; set; }

        [ForeignKey("FabricBatchId")]
        public virtual FabricBatch FabricBatch { get; set; }
    }
}