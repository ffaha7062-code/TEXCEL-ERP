using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a financial voucher (Journal, Cash, Bank).
    /// </summary>
    [Table("Vouchers")]
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoucherId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Voucher Number")]
        public string VoucherNumber { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Voucher Type")]
        public string VoucherType { get; set; } // Journal, Cash, Bank

        [Required]
        [Display(Name = "Voucher Date")]
        [DataType(DataType.Date)]
        public DateTime VoucherDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Narration")]
        [StringLength(1000)]
        public string Narration { get; set; }

        [Display(Name = "Total Debit")]
        public decimal TotalDebit { get; set; }

        [Display(Name = "Total Credit")]
        public decimal TotalCredit { get; set; }

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
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public virtual ICollection<VoucherDetail> VoucherDetails { get; set; }

        public Voucher()
        {
            VoucherDetails = new HashSet<VoucherDetail>();
        }
    }
}