using System;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Data.Repository
{
    /// <summary>
    /// Unit of Work implementation.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Companies = new Repository<Company>(_context);
            Branches = new Repository<Branch>(_context);
            Departments = new Repository<Department>(_context);
            Designations = new Repository<Designation>(_context);
            Employees = new Repository<Employee>(_context);
            Shifts = new Repository<Shift>(_context);
            Attendances = new Repository<Attendance>(_context);
            Leaves = new Repository<Leave>(_context);
            LeavePolicies = new Repository<LeavePolicy>(_context);
            SalaryHeads = new Repository<SalaryHead>(_context);
            SalaryStructures = new Repository<SalaryStructure>(_context);
            Payrolls = new Repository<Payroll>(_context);
            PayrollDetails = new Repository<PayrollDetail>(_context);
            Loans = new Repository<Loan>(_context);
            LoanInstallments = new Repository<LoanInstallment>(_context);
            Fabrics = new Repository<Fabric>(_context);
            FabricBatches = new Repository<FabricBatch>(_context);
            FabricStocks = new Repository<FabricStock>(_context);
            GRNs = new Repository<GRN>(_context);
            StockIssues = new Repository<StockIssue>(_context);
            StockTransfers = new Repository<StockTransfer>(_context);
            Suppliers = new Repository<Supplier>(_context);
            PurchaseOrders = new Repository<PurchaseOrder>(_context);
            PurchaseOrderDetails = new Repository<PurchaseOrderDetail>(_context);
            Customers = new Repository<Customer>(_context);
            SalesOrders = new Repository<SalesOrder>(_context);
            SalesOrderDetails = new Repository<SalesOrderDetail>(_context);
            Invoices = new Repository<Invoice>(_context);
            Dispatches = new Repository<Dispatch>(_context);
            ProductionPlans = new Repository<ProductionPlan>(_context);
            ProductionOrders = new Repository<ProductionOrder>(_context);
            Cuttings = new Repository<Cutting>(_context);
            Sewings = new Repository<Sewing>(_context);
            Finishings = new Repository<Finishing>(_context);
            QualityInspections = new Repository<QualityInspection>(_context);
            NCRs = new Repository<NCR>(_context);
            CAPAs = new Repository<CAPA>(_context);
            Accounts = new Repository<Account>(_context);
            Vouchers = new Repository<Voucher>(_context);
            VoucherDetails = new Repository<VoucherDetail>(_context);
            AuditLogs = new Repository<AuditLog>(_context);
            EmployeeDocuments = new Repository<EmployeeDocument>(_context);
            Notifications = new Repository<Notification>(_context);
            SystemSettings = new Repository<SystemSetting>(_context);
        }

        public IRepository<Company> Companies { get; private set; }
        public IRepository<Branch> Branches { get; private set; }
        public IRepository<Department> Departments { get; private set; }
        public IRepository<Designation> Designations { get; private set; }
        public IRepository<Employee> Employees { get; private set; }
        public IRepository<Shift> Shifts { get; private set; }
        public IRepository<Attendance> Attendances { get; private set; }
        public IRepository<Leave> Leaves { get; private set; }
        public IRepository<LeavePolicy> LeavePolicies { get; private set; }
        public IRepository<SalaryHead> SalaryHeads { get; private set; }
        public IRepository<SalaryStructure> SalaryStructures { get; private set; }
        public IRepository<Payroll> Payrolls { get; private set; }
        public IRepository<PayrollDetail> PayrollDetails { get; private set; }
        public IRepository<Loan> Loans { get; private set; }
        public IRepository<LoanInstallment> LoanInstallments { get; private set; }
        public IRepository<Fabric> Fabrics { get; private set; }
        public IRepository<FabricBatch> FabricBatches { get; private set; }
        public IRepository<FabricStock> FabricStocks { get; private set; }
        public IRepository<GRN> GRNs { get; private set; }
        public IRepository<StockIssue> StockIssues { get; private set; }
        public IRepository<StockTransfer> StockTransfers { get; private set; }
        public IRepository<Supplier> Suppliers { get; private set; }
        public IRepository<PurchaseOrder> PurchaseOrders { get; private set; }
        public IRepository<PurchaseOrderDetail> PurchaseOrderDetails { get; private set; }
        public IRepository<Customer> Customers { get; private set; }
        public IRepository<SalesOrder> SalesOrders { get; private set; }
        public IRepository<SalesOrderDetail> SalesOrderDetails { get; private set; }
        public IRepository<Invoice> Invoices { get; private set; }
        public IRepository<Dispatch> Dispatches { get; private set; }
        public IRepository<ProductionPlan> ProductionPlans { get; private set; }
        public IRepository<ProductionOrder> ProductionOrders { get; private set; }
        public IRepository<Cutting> Cuttings { get; private set; }
        public IRepository<Sewing> Sewings { get; private set; }
        public IRepository<Finishing> Finishings { get; private set; }
        public IRepository<QualityInspection> QualityInspections { get; private set; }
        public IRepository<NCR> NCRs { get; private set; }
        public IRepository<CAPA> CAPAs { get; private set; }
        public IRepository<Account> Accounts { get; private set; }
        public IRepository<Voucher> Vouchers { get; private set; }
        public IRepository<VoucherDetail> VoucherDetails { get; private set; }
        public IRepository<AuditLog> AuditLogs { get; private set; }
        public IRepository<EmployeeDocument> EmployeeDocuments { get; private set; }
        public IRepository<Notification> Notifications { get; private set; }
        public IRepository<SystemSetting> SystemSettings { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}