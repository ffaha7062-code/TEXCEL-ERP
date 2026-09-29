using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a production plan for a sales order.
    /// </summary>
    [Table("ProductionPlans")]
    public class ProductionPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductionPlanId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Plan Number")]
        public string PlanNumber { get; set; }

        [Required]
        [Display(Name = "Sales Order")]
        public int SalesOrderId { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Style")]
        [StringLength(100)]
        public string Style { get; set; }

        [Display(Name = "Plan Date")]
        [DataType(DataType.Date)]
        public DateTime PlanDate { get; set; } = DateTime.Now;

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Planned Quantity")]
        public int PlannedQuantity { get; set; }

        [Display(Name = "Produced Quantity")]
        public int ProducedQuantity { get; set; } = 0;

        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, OnHold

        [Display(Name = "Priority")]
        [StringLength(20)]
        public string Priority { get; set; } = "Normal"; // Normal, High, Urgent

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("SalesOrderId")]
        public virtual SalesOrder SalesOrder { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public virtual ICollection<ProductionOrder> ProductionOrders { get; set; }

        public ProductionPlan()
        {
            ProductionOrders = new HashSet<ProductionOrder>();
        }
    }
}