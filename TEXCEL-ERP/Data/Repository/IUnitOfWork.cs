using System;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Data.Repository
{
    /// <summary>
    /// Unit of Work interface.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Company> Companies { get; }
        IRepository<Branch> Branches { get; }
        IRepository<Department> Departments { get; }
        IRepository<Designation> Designations { get; }
        IRepository<Employee> Employees { get; }
        IRepository<Shift> Shifts { get; }
        IRepository<Attendance> Attendances { get; }

        IRepository<Holiday> Holidays { get; }
        IRepository<Leave> Leaves { get; }
        IRepository<LeavePolicy> LeavePolicies { get; }
        IRepository<SalaryHead> SalaryHeads { get; }
        IRepository<SalaryStructure> SalaryStructures { get; }
        IRepository<Payroll> Payrolls { get; }
        IRepository<PayrollDetail> PayrollDetails { get; }
        IRepository<Loan> Loans { get; }
        IRepository<LoanInstallment> LoanInstallments { get; }
        IRepository<Fabric> Fabrics { get; }
        IRepository<FabricBatch> FabricBatches { get; }
        IRepository<FabricStock> FabricStocks { get; }
        IRepository<GRN> GRNs { get; }
        IRepository<StockIssue> StockIssues { get; }
        IRepository<StockTransfer> StockTransfers { get; }
        IRepository<Supplier> Suppliers { get; }
        IRepository<PurchaseOrder> PurchaseOrders { get; }
        IRepository<PurchaseOrderDetail> PurchaseOrderDetails { get; }
        IRepository<Customer> Customers { get; }
        IRepository<SalesOrder> SalesOrders { get; }
        IRepository<SalesOrderDetail> SalesOrderDetails { get; }
        IRepository<Invoice> Invoices { get; }
        IRepository<Dispatch> Dispatches { get; }
        IRepository<ProductionPlan> ProductionPlans { get; }
        IRepository<ProductionOrder> ProductionOrders { get; }
        IRepository<Cutting> Cuttings { get; }
        IRepository<Sewing> Sewings { get; }
        IRepository<Finishing> Finishings { get; }
        IRepository<QualityInspection> QualityInspections { get; }
        IRepository<NCR> NCRs { get; }
        IRepository<CAPA> CAPAs { get; }
        IRepository<Account> Accounts { get; }
        IRepository<Voucher> Vouchers { get; }
        IRepository<VoucherDetail> VoucherDetails { get; }
        IRepository<AuditLog> AuditLogs { get; }
        IRepository<EmployeeDocument> EmployeeDocuments { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<SystemSetting> SystemSettings { get; }

        int Complete();
    }
}