using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a line item in a voucher.
    /// </summary>
    [Table("VoucherDetails")]
    public class VoucherDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoucherDetailId { get; set; }

        [Required]
        [Display(Name = "Voucher")]
        public int VoucherId { get; set; }

        [Required]
        [Display(Name = "Account")]
        public int AccountId { get; set; }

        [Display(Name = "Debit Amount")]
        public decimal DebitAmount { get; set; } = 0;

        [Display(Name = "Credit Amount")]
        public decimal CreditAmount { get; set; } = 0;

        [Display(Name = "Description")]
        [StringLength(500)]
        public string Description { get; set; }

        // Navigation Properties
        [ForeignKey("VoucherId")]
        public virtual Voucher Voucher { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}