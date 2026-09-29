using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a purchase order placed with a supplier.
    /// </summary>
    [Table("PurchaseOrders")]
    public class PurchaseOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "PO Number")]
        public string PONumber { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "PO Date")]
        [DataType(DataType.Date)]
        public DateTime PODate { get; set; } = DateTime.Now;

        [Display(Name = "Expected Delivery Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [Display(Name = "Status")]
        public OrderStatus Status { get; set; } = OrderStatus.Draft;

        [Display(Name = "Sub Total")]
        public decimal SubTotal { get; set; }

        [Display(Name = "Tax Amount")]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Approved By")]
        public int? ApprovedById { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedById { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public PurchaseOrder()
        {
            PurchaseOrderDetails = new HashSet<PurchaseOrderDetail>();
        }
    }
}