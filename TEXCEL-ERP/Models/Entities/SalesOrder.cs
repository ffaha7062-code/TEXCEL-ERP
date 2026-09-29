using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a sales order from a customer.
    /// </summary>
    [Table("SalesOrders")]
    public class SalesOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesOrderId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "SO Number")]
        public string SONumber { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Delivery Date")]
        [DataType(DataType.Date)]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Status")]
        public OrderStatus Status { get; set; } = OrderStatus.Draft;

        [Display(Name = "Order Type")]
        [StringLength(50)]
        public string OrderType { get; set; } // FOB, CIF, ExWorks

        [Display(Name = "Currency")]
        [StringLength(10)]
        public string Currency { get; set; } = "PKR";

        [Display(Name = "Sub Total")]
        public decimal SubTotal { get; set; }

        [Display(Name = "Tax Amount")]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Freight Amount")]
        public decimal FreightAmount { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Payment Terms")]
        [StringLength(100)]
        public string PaymentTerms { get; set; }

        [StringLength(500)]
        [Display(Name = "Shipping Mark")]
        public string ShippingMark { get; set; }

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
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
        public virtual ICollection<Dispatch> Dispatches { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }

        public SalesOrder()
        {
            SalesOrderDetails = new HashSet<SalesOrderDetail>();
            Dispatches = new HashSet<Dispatch>();
            Invoices = new HashSet<Invoice>();
        }
    }
}