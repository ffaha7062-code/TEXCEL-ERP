using System;
using System.Collections.Generic;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for dashboard KPIs, charts, recent orders, and tasks.
    /// </summary>
    public class DashboardViewModel
    {
        // =============================================
        // EMPLOYEE STATS
        // =============================================
        public int TotalEmployees { get; set; }
        public int PresentToday { get; set; }
        public int AbsentToday { get; set; }
        public int OnLeaveToday { get; set; }
        public decimal AttendancePercentage { get; set; }

        // =============================================
        // PAYROLL STATS
        // =============================================
        public decimal MonthlyPayroll { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal NetProfit { get; set; }

        // =============================================
        // INVENTORY STATS
        // =============================================
        public decimal TotalFabricStock { get; set; }
        public int TotalFabricRolls { get; set; }
        public int LowStockItems { get; set; }

        // =============================================
        // PRODUCTION STATS
        // =============================================
        public int MonthlyProduction { get; set; }
        public decimal ProductionEfficiency { get; set; }
        public decimal RejectionRate { get; set; }

        // =============================================
        // SALES STATS
        // =============================================
        public decimal MonthlySales { get; set; }
        public int PendingOrders { get; set; }
        public decimal OrderFulfillment { get; set; }

        // =============================================
        // PURCHASE STATS
        // =============================================
        public decimal MonthlyPurchase { get; set; }
        public int PendingPOs { get; set; }

        // =============================================
        // RECENT ACTIVITIES
        // =============================================
        public List<RecentOrder> RecentOrders { get; set; }
        public List<RecentTask> RecentTasks { get; set; }
        public List<NotificationItem> Notifications { get; set; }

        // =============================================
        // CHARTS DATA
        // =============================================
        public List<ChartData> MonthlySalesChart { get; set; }
        public List<ChartData> MonthlyProductionChart { get; set; }
        public List<ChartData> MonthlyPurchaseChart { get; set; }
        public List<ChartData> FabricStockChart { get; set; }

        // =============================================
        // BRANCH-WISE DATA
        // =============================================
        public List<BranchPerformance> BranchPerformances { get; set; }

        // =============================================
        // USER INFO
        // =============================================
        public string UserName { get; set; }
        public string BranchName { get; set; }
        public string Role { get; set; }
    }

    /// <summary>
    /// Recent order item for dashboard.
    /// </summary>
    public class RecentOrder
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public DateTime Date { get; set; }
        public string DateFormatted { get; set; }
    }

    /// <summary>
    /// Recent task item for dashboard.
    /// </summary>
    public class RecentTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Priority { get; set; }
        public string PriorityColor { get; set; }
        public DateTime DueDate { get; set; }
        public string DueDateFormatted { get; set; }
        public bool IsCompleted { get; set; }
    }

    /// <summary>
    /// Notification item for dashboard.
    /// </summary>
    public class NotificationItem
    {
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TimeAgo { get; set; }
    }

    /// <summary>
    /// Chart data item (label + value).
    /// </summary>
    public class ChartData
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
    }

    /// <summary>
    /// Branch performance data.
    /// </summary>
    public class BranchPerformance
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal Sales { get; set; }
        public decimal Production { get; set; }
        public int Employees { get; set; }
        public decimal Efficiency { get; set; }
    }
}