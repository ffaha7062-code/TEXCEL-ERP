using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Enums;
using TEXCEL_ERP.Models.ViewModels;
using TEXCEL_ERP.Helpers;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Dashboard business logic.
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =============================================
        // MAIN DASHBOARD DATA (All Branches — SuperAdmin)
        // =============================================
        public DashboardViewModel GetDashboardData()
        {
            var today = DateTime.Now.Date;
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var model = new DashboardViewModel
            {
                // EMPLOYEE STATS
                TotalEmployees = _unitOfWork.Employees.Count(e => e.IsActive),
                PresentToday = _unitOfWork.Attendances.Count(a => a.AttendanceDate == today && a.Status == AttendanceStatus.Present),
                AbsentToday = _unitOfWork.Attendances.Count(a => a.AttendanceDate == today && a.Status == AttendanceStatus.Absent),
                OnLeaveToday = _unitOfWork.Attendances.Count(a => a.AttendanceDate == today && a.Status == AttendanceStatus.Leave),

                // PAYROLL STATS
                MonthlyPayroll = _unitOfWork.Payrolls
                    .Find(p => p.Month == currentMonth && p.Year == currentYear)
                    .Sum(p => p.NetSalary),

                // INVENTORY STATS
                TotalFabricStock = _unitOfWork.FabricStocks.GetAll().Sum(s => s.Quantity),
                TotalFabricRolls = _unitOfWork.FabricStocks.Count(),

                // SALES STATS
                MonthlySales = _unitOfWork.Invoices
                    .Find(i => i.InvoiceDate.Month == currentMonth && i.InvoiceDate.Year == currentYear)
                    .Sum(i => i.TotalAmount),
                PendingOrders = _unitOfWork.SalesOrders.Count(o => o.Status == OrderStatus.Approved || o.Status == OrderStatus.InProduction),

                // PRODUCTION STATS
                MonthlyProduction = _unitOfWork.ProductionOrders
                    .Find(p => p.CreatedDate.Month == currentMonth && p.CreatedDate.Year == currentYear)
                    .Sum(p => p.ProducedQuantity),

                // PURCHASE STATS
                MonthlyPurchase = _unitOfWork.PurchaseOrders
                    .Find(p => p.PODate.Month == currentMonth && p.PODate.Year == currentYear)
                    .Sum(p => p.TotalAmount),
                PendingPOs = _unitOfWork.PurchaseOrders.Count(o => o.Status == OrderStatus.Approved || o.Status == OrderStatus.Draft)
            };

            // ATTENDANCE PERCENTAGE
            if (model.TotalEmployees > 0)
            {
                model.AttendancePercentage = Math.Round((decimal)model.PresentToday / model.TotalEmployees * 100, 1);
            }

            // PRODUCTION EFFICIENCY
            var productionOrders = _unitOfWork.ProductionOrders.GetAll().ToList();
            if (productionOrders.Any())
            {
                var totalPlanned = productionOrders.Sum(p => p.PlannedQuantity);
                var totalProduced = productionOrders.Sum(p => p.ProducedQuantity);
                if (totalPlanned > 0)
                {
                    model.ProductionEfficiency = Math.Round((decimal)totalProduced / totalPlanned * 100, 1);
                }
            }

            // RECENT ACTIVITIES
            model.RecentOrders = GetRecentOrders();
            model.RecentTasks = GetRecentTasks();
            model.Notifications = GetNotifications();

            // CHARTS DATA
            model.MonthlySalesChart = GetMonthlySalesChart();
            model.MonthlyProductionChart = GetMonthlyProductionChart();
            model.MonthlyPurchaseChart = GetMonthlyPurchaseChart();

            // BRANCH PERFORMANCES (only for SuperAdmin)
            model.BranchPerformances = GetBranchPerformances();

            return model;
        }

        // =============================================
        // BRANCH DASHBOARD DATA (BranchAdmin / Manager)
        // =============================================
        public DashboardViewModel GetBranchDashboardData(int branchId)
        {
            var today = DateTime.Now.Date;
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var employeeIds = _unitOfWork.Employees
                .Find(e => e.BranchId == branchId)
                .Select(e => e.EmployeeId)
                .ToList();

            var model = new DashboardViewModel
            {
                // EMPLOYEE STATS
                TotalEmployees = _unitOfWork.Employees.Count(e => e.BranchId == branchId && e.IsActive),
                PresentToday = _unitOfWork.Attendances
                    .Find(a => a.AttendanceDate == today && employeeIds.Contains(a.EmployeeId) && a.Status == AttendanceStatus.Present)
                    .Count(),
                AbsentToday = _unitOfWork.Attendances
                    .Find(a => a.AttendanceDate == today && employeeIds.Contains(a.EmployeeId) && a.Status == AttendanceStatus.Absent)
                    .Count(),
                OnLeaveToday = _unitOfWork.Attendances
                    .Find(a => a.AttendanceDate == today && employeeIds.Contains(a.EmployeeId) && a.Status == AttendanceStatus.Leave)
                    .Count(),

                // PAYROLL STATS
                MonthlyPayroll = _unitOfWork.Payrolls
                    .Find(p => employeeIds.Contains(p.EmployeeId) && p.Month == currentMonth && p.Year == currentYear)
                    .Sum(p => p.NetSalary),

                // INVENTORY STATS
                TotalFabricStock = _unitOfWork.FabricStocks
                    .Find(s => s.BranchId == branchId)
                    .Sum(s => s.Quantity),
                TotalFabricRolls = _unitOfWork.FabricStocks.Count(s => s.BranchId == branchId),

                // SALES STATS
                MonthlySales = _unitOfWork.Invoices
                    .Find(i => i.BranchId == branchId && i.InvoiceDate.Month == currentMonth && i.InvoiceDate.Year == currentYear)
                    .Sum(i => i.TotalAmount),
                PendingOrders = _unitOfWork.SalesOrders.Count(o => o.BranchId == branchId
                    && (o.Status == OrderStatus.Approved || o.Status == OrderStatus.InProduction)),

                // PRODUCTION STATS
                MonthlyProduction = _unitOfWork.ProductionOrders
                    .Find(p => p.BranchId == branchId
                        && p.CreatedDate.Month == currentMonth
                        && p.CreatedDate.Year == currentYear)
                    .Sum(p => p.ProducedQuantity),

                // PURCHASE STATS
                MonthlyPurchase = _unitOfWork.PurchaseOrders
                    .Find(p => p.BranchId == branchId
                        && p.PODate.Month == currentMonth
                        && p.PODate.Year == currentYear)
                    .Sum(p => p.TotalAmount),
                PendingPOs = _unitOfWork.PurchaseOrders.Count(o => o.BranchId == branchId
                    && (o.Status == OrderStatus.Approved || o.Status == OrderStatus.Draft))
            };

            // ATTENDANCE PERCENTAGE
            if (model.TotalEmployees > 0)
            {
                model.AttendancePercentage = Math.Round((decimal)model.PresentToday / model.TotalEmployees * 100, 1);
            }

            // PRODUCTION EFFICIENCY (branch-specific)
            var productionOrders = _unitOfWork.ProductionOrders
                .Find(p => p.BranchId == branchId)
                .ToList();
            if (productionOrders.Any())
            {
                var totalPlanned = productionOrders.Sum(p => p.PlannedQuantity);
                var totalProduced = productionOrders.Sum(p => p.ProducedQuantity);
                if (totalPlanned > 0)
                {
                    model.ProductionEfficiency = Math.Round((decimal)totalProduced / totalPlanned * 100, 1);
                }
            }

            // RECENT ACTIVITIES
            model.RecentOrders = GetRecentOrders(branchId);
            model.RecentTasks = GetRecentTasks();
            model.Notifications = GetNotifications();

            // CHARTS DATA (branch-specific)
            model.MonthlySalesChart = GetMonthlySalesChart(branchId);
            model.MonthlyProductionChart = GetMonthlyProductionChart(branchId);
            model.MonthlyPurchaseChart = GetMonthlyPurchaseChart(branchId);

            // BranchPerformances — BranchAdmin/Manager ko nahi chahiye
            model.BranchPerformances = new List<BranchPerformance>();

            return model;
        }

        // =============================================
        // PRIVATE HELPERS
        // =============================================

        private List<RecentOrder> GetRecentOrders(int? branchId = null)
        {
            var query = _unitOfWork.SalesOrders.GetAll().AsQueryable();

            if (branchId.HasValue)
                query = query.Where(o => o.BranchId == branchId.Value);

            return query
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList()
                .Select(o => new RecentOrder
                {
                    OrderId = o.SalesOrderId,
                    OrderNumber = o.SONumber ?? "N/A",
                    CustomerName = o.Customer != null ? o.Customer.CustomerName : "N/A",
                    Amount = o.TotalAmount,
                    Status = o.Status.ToString(),
                    StatusColor = GetStatusColor(o.Status),
                    Date = o.OrderDate,
                    DateFormatted = o.OrderDate.ToString("dd MMM yyyy")
                })
                .ToList();
        }

        private List<RecentTask> GetRecentTasks()
        {
            // Static tasks for now — later can be from database
            return new List<RecentTask>
            {
                new RecentTask
                {
                    TaskId = 1,
                    Title = "Review Payroll for August",
                    Priority = "High",
                    PriorityColor = "#ef4444",
                    DueDate = DateTime.Now.Date.AddHours(10),
                    DueDateFormatted = "Today · 10:00 AM",
                    IsCompleted = true
                },
                new RecentTask
                {
                    TaskId = 2,
                    Title = "Approve Purchase Order #PO-0422",
                    Priority = "Medium",
                    PriorityColor = "#f59e0b",
                    DueDate = DateTime.Now.Date.AddHours(11.75),
                    DueDateFormatted = "Today · 11:45 AM",
                    IsCompleted = false
                },
                new RecentTask
                {
                    TaskId = 3,
                    Title = "Production Meeting — Q3 Planning",
                    Priority = "High",
                    PriorityColor = "#ef4444",
                    DueDate = DateTime.Now.Date.AddHours(15),
                    DueDateFormatted = "Today · 3:00 PM",
                    IsCompleted = false
                },
                new RecentTask
                {
                    TaskId = 4,
                    Title = "Fabric QC Report Review",
                    Priority = "Low",
                    PriorityColor = "#10b981",
                    DueDate = DateTime.Now.Date.AddHours(17.5),
                    DueDateFormatted = "Today · 5:30 PM",
                    IsCompleted = false
                }
            };
        }

        private List<NotificationItem> GetNotifications()
        {
            return _unitOfWork.Notifications.GetAll()
                .OrderByDescending(n => n.CreatedDate)
                .Take(5)
                .ToList()
                .Select(n => new NotificationItem
                {
                    NotificationId = n.NotificationId,
                    Title = n.Title ?? "Notification",
                    Message = n.Message ?? "",
                    Type = n.Type ?? "Info",
                    Icon = GetNotificationIcon(n.Type),
                    Color = GetNotificationColor(n.Type),
                    CreatedDate = n.CreatedDate,
                    TimeAgo = GetTimeAgo(n.CreatedDate)
                })
                .ToList();
        }

        // =============================================
        // CHART METHODS (Optional branchId parameter)
        // =============================================

        private List<ChartData> GetMonthlySalesChart(int? branchId = null)
        {
            var currentYear = DateTime.Now.Year;
            var data = new List<ChartData>();

            for (int month = 1; month <= 12; month++)
            {
                var query = _unitOfWork.Invoices
                    .Find(i => i.InvoiceDate.Year == currentYear && i.InvoiceDate.Month == month);

                if (branchId.HasValue)
                {
                    int bid = branchId.Value;
                    query = query.Where(i => i.BranchId == bid);
                }

                var total = query.Sum(i => i.TotalAmount);

                data.Add(new ChartData
                {
                    Label = CommonHelper.GetMonthName(month).Substring(0, 3),
                    Value = total
                });
            }

            return data;
        }

        private List<ChartData> GetMonthlyProductionChart(int? branchId = null)
        {
            var currentYear = DateTime.Now.Year;
            var data = new List<ChartData>();

            for (int month = 1; month <= 12; month++)
            {
                var query = _unitOfWork.ProductionOrders
                    .Find(p => p.CreatedDate.Year == currentYear && p.CreatedDate.Month == month);

                if (branchId.HasValue)
                {
                    int bid = branchId.Value;
                    query = query.Where(p => p.BranchId == bid);
                }

                var total = query.Sum(p => p.ProducedQuantity);

                data.Add(new ChartData
                {
                    Label = CommonHelper.GetMonthName(month).Substring(0, 3),
                    Value = total
                });
            }

            return data;
        }

        private List<ChartData> GetMonthlyPurchaseChart(int? branchId = null)
        {
            var currentYear = DateTime.Now.Year;
            var data = new List<ChartData>();

            for (int month = 1; month <= 12; month++)
            {
                var query = _unitOfWork.PurchaseOrders
                    .Find(p => p.PODate.Year == currentYear && p.PODate.Month == month);

                if (branchId.HasValue)
                {
                    int bid = branchId.Value;
                    query = query.Where(p => p.BranchId == bid);
                }

                var total = query.Sum(p => p.TotalAmount);

                data.Add(new ChartData
                {
                    Label = CommonHelper.GetMonthName(month).Substring(0, 3),
                    Value = total
                });
            }

            return data;
        }

        private List<BranchPerformance> GetBranchPerformances()
        {
            var branches = _unitOfWork.Branches.GetAll().Where(b => b.IsActive).ToList();
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            return branches.Select(b => new BranchPerformance
            {
                BranchId = b.BranchId,
                BranchName = b.BranchName,
                Sales = _unitOfWork.Invoices
                    .Find(i => i.BranchId == b.BranchId && i.InvoiceDate.Month == currentMonth && i.InvoiceDate.Year == currentYear)
                    .Sum(i => i.TotalAmount),
                Production = _unitOfWork.ProductionOrders
                    .Find(p => p.BranchId == b.BranchId && p.CreatedDate.Month == currentMonth && p.CreatedDate.Year == currentYear)
                    .Sum(p => p.ProducedQuantity),
                Employees = _unitOfWork.Employees.Count(e => e.BranchId == b.BranchId && e.IsActive),
                Efficiency = 75 // placeholder — later calculate
            }).ToList();
        }

        // =============================================
        // COLOR / ICON / TIME HELPERS
        // =============================================

        private string GetStatusColor(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Delivered:
                case OrderStatus.Closed:
                    return "#10b981"; // Green
                case OrderStatus.InProduction:
                    return "#f59e0b"; // Amber
                case OrderStatus.Approved:
                    return "#3b82f6"; // Blue
                case OrderStatus.Draft:
                    return "#6b7280"; // Gray
                case OrderStatus.Cancelled:
                    return "#ef4444"; // Red
                default:
                    return "#6b7280";
            }
        }

        private string GetNotificationIcon(string type)
        {
            switch (type)
            {
                case "Success": return "fa-check-circle";
                case "Warning": return "fa-exclamation-triangle";
                case "Error": return "fa-times-circle";
                default: return "fa-info-circle";
            }
        }

        private string GetNotificationColor(string type)
        {
            switch (type)
            {
                case "Success": return "#10b981";
                case "Warning": return "#f59e0b";
                case "Error": return "#ef4444";
                default: return "#3b82f6";
            }
        }

        private string GetTimeAgo(DateTime date)
        {
            var span = DateTime.Now - date;
            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} min ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours} hours ago";
            if (span.TotalDays < 30) return $"{(int)span.TotalDays} days ago";
            return date.ToString("dd MMM yyyy");
        }
    }
}