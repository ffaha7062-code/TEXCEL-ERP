using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a customer.
    /// </summary>
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [StringLength(100)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [StringLength(20)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(500)]
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }

        [StringLength(500)]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }

        [StringLength(100)]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(50)]
        [Display(Name = "NTN")]
        public string NTN { get; set; }

        [StringLength(50)]
        [Display(Name = "GST Number")]
        public string GSTNumber { get; set; }

        [Display(Name = "Customer Type")]
        [StringLength(50)]
        public string CustomerType { get; set; } // Local, Export, Wholesale, Retail

        [Display(Name = "Credit Limit")]
        public decimal CreditLimit { get; set; } = 0;

        [Display(Name = "Credit Days")]
        public int CreditDays { get; set; } = 0;

        [Display(Name = "Payment Terms")]
        [StringLength(100)]
        public string PaymentTerms { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }

        public Customer()
        {
            SalesOrders = new HashSet<SalesOrder>();
            Invoices = new HashSet<Invoice>();
        }
    }
}