using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a Goods Receipt Note (GRN) against a Purchase Order.
    /// </summary>
    [Table("GRNs")]
    public class GRN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GRNId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "GRN Number")]
        public string GRNNumber { get; set; }

        [Display(Name = "Purchase Order")]
        public int? PurchaseOrderId { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        [Required]
        [Display(Name = "Fabric")]
        public int FabricId { get; set; }

        [Display(Name = "Fabric Batch")]
        public int? FabricBatchId { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Receipt Date")]
        [DataType(DataType.Date)]
        public DateTime ReceiptDate { get; set; } = DateTime.Now;

        [Display(Name = "Quantity Received")]
        public decimal Quantity { get; set; }

        [Display(Name = "Unit of Measurement")]
        [StringLength(20)]
        public string UOM { get; set; } = "Meter";

        [Display(Name = "Rate Per Unit")]
        public decimal RatePerUnit { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount => Quantity * RatePerUnit;

        [Display(Name = "Roll Count")]
        public int? RollCount { get; set; }

        [Display(Name = "Quality Status")]
        [StringLength(20)]
        public string QualityStatus { get; set; } = "Pending"; // Pass, Fail, Rework, Pending

        [StringLength(500)]
        [Display(Name = "QC Remarks")]
        public string QCRemarks { get; set; }

        [Display(Name = "Warehouse")]
        [StringLength(100)]
        public string Warehouse { get; set; }

        [Display(Name = "Location")]
        [StringLength(100)]
        public string Location { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Approved By")]
        public int? ApprovedById { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey("FabricId")]
        public virtual Fabric Fabric { get; set; }

        [ForeignKey("FabricBatchId")]
        public virtual FabricBatch FabricBatch { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
    }
}