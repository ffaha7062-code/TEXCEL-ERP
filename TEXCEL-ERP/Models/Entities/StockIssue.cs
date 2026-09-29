using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a stock issue for production or other purposes.
    /// </summary>
    [Table("StockIssues")]
    public class StockIssue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockIssueId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Issue Number")]
        public string IssueNumber { get; set; }

        [Required]
        [Display(Name = "Fabric")]
        public int FabricId { get; set; }

        [Display(Name = "Fabric Batch")]
        public int? FabricBatchId { get; set; }

        [Required]
        [Display(Name = "From Branch")]
        public int FromBranchId { get; set; }

        [Display(Name = "To Department")]
        [StringLength(100)]
        public string ToDepartment { get; set; }

        [Display(Name = "Purpose")]
        [StringLength(200)]
        public string Purpose { get; set; } // Production, Sample, R&D

        [Display(Name = "Reference")]
        [StringLength(100)]
        public string Reference { get; set; } // Order Number / Production Order

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Display(Name = "Quantity Issued")]
        public decimal Quantity { get; set; }

        [Display(Name = "Unit of Measurement")]
        [StringLength(20)]
        public string UOM { get; set; } = "Meter";

        [Display(Name = "Issued By")]
        public int? IssuedById { get; set; }

        [Display(Name = "Received By")]
        [StringLength(100)]
        public string ReceivedBy { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Approved By")]
        public int? ApprovedById { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("FabricId")]
        public virtual Fabric Fabric { get; set; }

        [ForeignKey("FabricBatchId")]
        public virtual FabricBatch FabricBatch { get; set; }

        [ForeignKey("FromBranchId")]
        public virtual Branch FromBranch { get; set; }
    }
}