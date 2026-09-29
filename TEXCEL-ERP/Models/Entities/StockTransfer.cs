using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a stock transfer between branches.
    /// </summary>
    [Table("StockTransfers")]
    public class StockTransfer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockTransferId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Transfer Number")]
        public string TransferNumber { get; set; }

        [Required]
        [Display(Name = "Fabric")]
        public int FabricId { get; set; }

        [Display(Name = "Fabric Batch")]
        public int? FabricBatchId { get; set; }

        [Required]
        [Display(Name = "From Branch")]
        public int FromBranchId { get; set; }

        [Required]
        [Display(Name = "To Branch")]
        public int ToBranchId { get; set; }

        [Display(Name = "Transfer Date")]
        [DataType(DataType.Date)]
        public DateTime TransferDate { get; set; } = DateTime.Now;

        // ✅ YEH PROPERTY ADD KAREIN — jo missing thi
        [Required]
        [Display(Name = "Quantity")]
        [Range(0.001, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }

        [Display(Name = "Unit of Measurement")]
        [StringLength(20)]
        public string UOM { get; set; } = "Meter";

        [Display(Name = "Vehicle Number")]
        [StringLength(50)]
        public string VehicleNumber { get; set; }

        [Display(Name = "Driver Name")]
        [StringLength(100)]
        public string DriverName { get; set; }

        [Display(Name = "Expected Delivery Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, InTransit, Received, Cancelled

        [Display(Name = "Received Date")]
        public DateTime? ReceivedDate { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Approved By")]
        public int? ApprovedById { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // =============================================
        // NAVIGATION PROPERTIES (NO [ForeignKey] attributes)
        // =============================================
        public virtual Fabric Fabric { get; set; }
        public virtual FabricBatch FabricBatch { get; set; }
        public virtual Branch FromBranch { get; set; }
        public virtual Branch ToBranch { get; set; }
    }
}