using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a chart of account.
    /// </summary>
    [Table("Accounts")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Account Type")]
        public string AccountType { get; set; } // Asset, Liability, Equity, Income, Expense

        [Display(Name = "Parent Account")]
        public int? ParentAccountId { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Opening Balance")]
        public decimal OpeningBalance { get; set; } = 0;

        [Display(Name = "Current Balance")]
        public decimal CurrentBalance { get; set; } = 0;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("ParentAccountId")]
        public virtual Account ParentAccount { get; set; }

        public virtual ICollection<Account> ChildAccounts { get; set; }
        public virtual ICollection<VoucherDetail> VoucherDetails { get; set; }

        public Account()
        {
            ChildAccounts = new HashSet<Account>();
            VoucherDetails = new HashSet<VoucherDetail>();
        }
    }
}