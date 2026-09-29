using TEXCEL_ERP.Models.ViewModels;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Dashboard business logic.
    /// </summary>
    public interface IDashboardService
    {
        DashboardViewModel GetDashboardData();
        DashboardViewModel GetBranchDashboardData(int branchId);
    }
}