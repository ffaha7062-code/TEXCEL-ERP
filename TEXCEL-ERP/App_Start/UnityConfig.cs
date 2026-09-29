using System.Web.Mvc;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Services;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;   // ← Unity.Mvc5 ka namespace

namespace TEXCEL_ERP.App_Start
{
    /// <summary>
    /// Unity Dependency Injection Configuration.
    /// </summary>
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Register DbContext
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());

            // Register UnitOfWork
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());

            // Register Services
            container.RegisterType<IEmployeeService, EmployeeService>();
            container.RegisterType<IAttendanceService, AttendanceService>();
            container.RegisterType<IPayrollService, PayrollService>();
            container.RegisterType<ILeaveService, LeaveService>();
            container.RegisterType<IInventoryService, InventoryService>();
            container.RegisterType<ISalesService, SalesService>();
            container.RegisterType<IProductionService, ProductionService>();
            container.RegisterType<IDashboardService, DashboardService>();
            // Services
            container.RegisterType<IBranchService, BranchService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ICompanyService, CompanyService>();
           

            // ✅ FULLY QUALIFIED — Ambiguity khatam
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
        }
    }
}