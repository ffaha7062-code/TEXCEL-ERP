using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a production order (Cutting, Sewing, Finishing).
    /// </summary>
    [Table("ProductionOrders")]
    public class ProductionOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductionOrderId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }

        [Required]
        [Display(Name = "Production Plan")]
        public int ProductionPlanId { get; set; }

        [Display(Name = "Stage")]
        [StringLength(50)]
        public string Stage { get; set; } // Cutting, Sewing, Finishing

        [Display(Name = "Line Number")]
        [StringLength(50)]
        public string LineNumber { get; set; }

        [Display(Name = "Supervisor")]
        [StringLength(100)]
        public string Supervisor { get; set; }

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

        [Display(Name = "Rejected Quantity")]
        public int RejectedQuantity { get; set; } = 0;

        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, OnHold

        [Display(Name = "Efficiency (%)")]
        public decimal? Efficiency { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(500)]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("ProductionPlanId")]
        public virtual ProductionPlan ProductionPlan { get; set; }
    }
}