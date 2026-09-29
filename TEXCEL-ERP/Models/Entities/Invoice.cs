using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents an invoice generated against a sales order.
    /// </summary>
    [Table("Invoices")]
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Sales Order")]
        public int? SalesOrderId { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Invoice Date")]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

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

        [Display(Name = "Paid Amount")]
        public decimal PaidAmount { get; set; } = 0;

        [Display(Name = "Outstanding Amount")]
        public decimal OutstandingAmount => TotalAmount - PaidAmount;

        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [Display(Name = "Payment Date")]
        public DateTime? PaymentDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("SalesOrderId")]
        public virtual SalesOrder SalesOrder { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
    }
}