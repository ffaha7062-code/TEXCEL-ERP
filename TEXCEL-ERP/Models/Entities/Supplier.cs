using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a supplier of fabric, yarn, or other materials.
    /// </summary>
    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Supplier Code")]
        public string SupplierCode { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

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
        [Display(Name = "Address")]
        public string Address { get; set; }

        [StringLength(100)]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(50)]
        [Display(Name = "NTN")]
        public string NTN { get; set; }

        [StringLength(50)]
        [Display(Name = "GST Number")]
        public string GSTNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [StringLength(50)]
        [Display(Name = "Bank Account Number")]
        public string BankAccountNumber { get; set; }

        [Display(Name = "Payment Terms")]
        [StringLength(100)]
        public string PaymentTerms { get; set; }

        [Display(Name = "Credit Limit")]
        public decimal CreditLimit { get; set; } = 0;

        [Display(Name = "Credit Days")]
        public int CreditDays { get; set; } = 0;

        [Display(Name = "Lead Time (Days)")]
        public int LeadTimeDays { get; set; } = 0;

        [Display(Name = "Rating")]
        [Range(1, 5)]
        public int Rating { get; set; } = 3;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<GRN> GRNs { get; set; }
        public virtual ICollection<FabricBatch> FabricBatches { get; set; }

        public Supplier()
        {
            PurchaseOrders = new HashSet<PurchaseOrder>();
            GRNs = new HashSet<GRN>();
            FabricBatches = new HashSet<FabricBatch>();
        }
    }
}