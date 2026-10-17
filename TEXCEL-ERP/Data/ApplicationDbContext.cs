using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data.Entity;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Data
{
    /// <summary>
    /// Application Database Context (Code-First).
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        // DbSets
        public DbSet<Company> Companies { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeavePolicy> LeavePolicies { get; set; }
        public DbSet<SalaryHead> SalaryHeads { get; set; }
        public DbSet<SalaryStructure> SalaryStructures { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollDetail> PayrollDetails { get; set; }
        public DbSet<Loan> Loans { get; set; }

        public DbSet<Advance> Advances { get; set; }
        public DbSet<AdvanceRecovery> AdvanceRecoveries { get; set; }
        public DbSet<TaxSlab> TaxSlabs { get; set; }
        public DbSet<LoanInstallment> LoanInstallments { get; set; }
        public DbSet<Fabric> Fabrics { get; set; }
        public DbSet<FabricBatch> FabricBatches { get; set; }
        public DbSet<FabricStock> FabricStocks { get; set; }
        public DbSet<GRN> GRNs { get; set; }
        public DbSet<StockIssue> StockIssues { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Dispatch> Dispatches { get; set; }
        public DbSet<ProductionPlan> ProductionPlans { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<Cutting> Cuttings { get; set; }
        public DbSet<Sewing> Sewings { get; set; }
        public DbSet<Finishing> Finishings { get; set; }
        public DbSet<QualityInspection> QualityInspections { get; set; }
        public DbSet<NCR> NCRs { get; set; }
        public DbSet<CAPA> CAPAs { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherDetail> VoucherDetails { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =============================================
            // DECIMAL PRECISION CONFIGURATION
            // =============================================
            modelBuilder.Entity<Employee>().Property(e => e.BasicSalary).HasPrecision(18, 2);
            modelBuilder.Entity<Payroll>().Property(p => p.BasicSalary).HasPrecision(18, 2);
            modelBuilder.Entity<Payroll>().Property(p => p.TotalEarnings).HasPrecision(18, 2);
            modelBuilder.Entity<Payroll>().Property(p => p.TotalDeductions).HasPrecision(18, 2);
            modelBuilder.Entity<Payroll>().Property(p => p.NetSalary).HasPrecision(18, 2);
            modelBuilder.Entity<Payroll>().Property(p => p.OvertimeAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Loan>().Property(l => l.LoanAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Loan>().Property(l => l.EMIAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Loan>().Property(l => l.TotalPayable).HasPrecision(18, 2);
            modelBuilder.Entity<Loan>().Property(l => l.TotalPaid).HasPrecision(18, 2);
            modelBuilder.Entity<Loan>().Property(l => l.OutstandingAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Fabric>().Property(f => f.RatePerUnit).HasPrecision(18, 2);
            modelBuilder.Entity<FabricStock>().Property(f => f.Quantity).HasPrecision(18, 3);
            modelBuilder.Entity<FabricStock>().Property(f => f.RatePerUnit).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.SubTotal).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.TaxAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.PaidAmount).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrder>().Property(s => s.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrder>().Property(s => s.SubTotal).HasPrecision(18, 2);
            modelBuilder.Entity<Shift>().Property(s => s.OvertimeRate).HasPrecision(18, 2);
            modelBuilder.Entity<PurchaseOrder>().Property(p => p.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<PurchaseOrder>().Property(p => p.SubTotal).HasPrecision(18, 2);
            modelBuilder.Entity<GRN>().Property(g => g.Quantity).HasPrecision(18, 3);
            modelBuilder.Entity<GRN>().Property(g => g.RatePerUnit).HasPrecision(18, 2);
            modelBuilder.Entity<StockIssue>().Property(s => s.Quantity).HasPrecision(18, 3);
            modelBuilder.Entity<StockTransfer>().Property(s => s.Quantity).HasPrecision(18, 3);
            modelBuilder.Entity<Voucher>().Property(v => v.TotalDebit).HasPrecision(18, 2);
            modelBuilder.Entity<Voucher>().Property(v => v.TotalCredit).HasPrecision(18, 2);
            modelBuilder.Entity<VoucherDetail>().Property(v => v.DebitAmount).HasPrecision(18, 2);
            modelBuilder.Entity<VoucherDetail>().Property(v => v.CreditAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Account>().Property(a => a.OpeningBalance).HasPrecision(18, 2);
            modelBuilder.Entity<Account>().Property(a => a.CurrentBalance).HasPrecision(18, 2);
            modelBuilder.Entity<SalaryStructure>().Property(s => s.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<SalaryHead>().Property(s => s.DefaultValue).HasPrecision(18, 2);
            modelBuilder.Entity<LoanInstallment>().Property(l => l.EMIAmount).HasPrecision(18, 2);
            modelBuilder.Entity<LoanInstallment>().Property(l => l.PrincipalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<LoanInstallment>().Property(l => l.InterestAmount).HasPrecision(18, 2);
            modelBuilder.Entity<LoanInstallment>().Property(l => l.PaidAmount).HasPrecision(18, 2);
        
            modelBuilder.Entity<Employee>().Property(e => e.PieceRate).HasPrecision(18, 2);


            // =============================================
            // UNIQUE CONSTRAINTS
            // =============================================
            modelBuilder.Entity<Employee>().HasIndex(e => e.EmployeeCode).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.CNIC).IsUnique();
            modelBuilder.Entity<Branch>().HasIndex(b => b.BranchCode).IsUnique();
            modelBuilder.Entity<Department>().HasIndex(d => d.DepartmentCode).IsUnique();
            modelBuilder.Entity<Designation>().HasIndex(d => d.DesignationCode).IsUnique();
            modelBuilder.Entity<Supplier>().HasIndex(s => s.SupplierCode).IsUnique();
            modelBuilder.Entity<Customer>().HasIndex(c => c.CustomerCode).IsUnique();
            modelBuilder.Entity<Fabric>().HasIndex(f => f.FabricCode).IsUnique();

            // =============================================
            // COMPANY → BRANCH
            // =============================================
            modelBuilder.Entity<Branch>()
                .HasRequired(b => b.Company)
                .WithMany(c => c.Branches)
                .HasForeignKey(b => b.CompanyId)
                .WillCascadeOnDelete(false);

            // =============================================
            // COMPANY → EMPLOYEE
            // =============================================
            modelBuilder.Entity<Employee>()
                .HasOptional(e => e.ReportingManager)
                .WithMany()
                .HasForeignKey(e => e.ReportingManagerId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → EMPLOYEE
            // =============================================
            modelBuilder.Entity<Employee>()
                .HasRequired(e => e.Branch)
                .WithMany(b => b.Employees)
                .HasForeignKey(e => e.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → DEPARTMENT
            // =============================================
            modelBuilder.Entity<Department>()
                .HasRequired(d => d.Branch)
                .WithMany(b => b.Departments)
                .HasForeignKey(d => d.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // DEPARTMENT → DESIGNATION
            // =============================================
            modelBuilder.Entity<Designation>()
                .HasRequired(d => d.Department)
                .WithMany(dep => dep.Designations)
                .HasForeignKey(d => d.DepartmentId)
                .WillCascadeOnDelete(false);

            // =============================================
            // DEPARTMENT → EMPLOYEE
            // =============================================
            modelBuilder.Entity<Employee>()
                .HasRequired(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .WillCascadeOnDelete(false);

            // =============================================
            // DESIGNATION → EMPLOYEE
            // =============================================
            modelBuilder.Entity<Employee>()
                .HasRequired(e => e.Designation)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DesignationId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SHIFT → EMPLOYEE
            // =============================================
            modelBuilder.Entity<Employee>()
                .HasOptional(e => e.Shift)
                .WithMany(s => s.Employees)
                .HasForeignKey(e => e.ShiftId)
                .WillCascadeOnDelete(false);

            // =============================================
            // EMPLOYEE → ATTENDANCE
            // =============================================
            modelBuilder.Entity<Attendance>()
                .HasRequired(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SHIFT → ATTENDANCE
            // =============================================
            modelBuilder.Entity<Attendance>()
                .HasOptional(a => a.Shift)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.ShiftId)
                .WillCascadeOnDelete(false);

            // =============================================
            // EMPLOYEE → LEAVE
            // =============================================
            modelBuilder.Entity<Leave>()
                .HasRequired(l => l.Employee)
                .WithMany(e => e.Leaves)
                .HasForeignKey(l => l.EmployeeId)
                .WillCascadeOnDelete(false);

            // =============================================
            // EMPLOYEE → PAYROLL
            // =============================================
            modelBuilder.Entity<Payroll>()
                .HasRequired(p => p.Employee)
                .WithMany(e => e.Payrolls)
                .HasForeignKey(p => p.EmployeeId)
                .WillCascadeOnDelete(false);

            // =============================================
            // PAYROLL → PAYROLL DETAIL
            // =============================================
            modelBuilder.Entity<PayrollDetail>()
                .HasRequired(p => p.Payroll)
                .WithMany(p => p.PayrollDetails)
                .HasForeignKey(p => p.PayrollId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SALARY HEAD → PAYROLL DETAIL
            // =============================================
            modelBuilder.Entity<PayrollDetail>()
                .HasRequired(p => p.SalaryHead)
                .WithMany(s => s.PayrollDetails)
                .HasForeignKey(p => p.SalaryHeadId)
                .WillCascadeOnDelete(false);

            // =============================================
            // EMPLOYEE → SALARY STRUCTURE
            // =============================================
            modelBuilder.Entity<SalaryStructure>()
                .HasRequired(s => s.Employee)
                .WithMany(e => e.SalaryStructures)
                .HasForeignKey(s => s.EmployeeId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SALARY HEAD → SALARY STRUCTURE
            // =============================================
            modelBuilder.Entity<SalaryStructure>()
                .HasRequired(s => s.SalaryHead)
                .WithMany(s => s.SalaryStructures)
                .HasForeignKey(s => s.SalaryHeadId)
                .WillCascadeOnDelete(false);

            // =============================================
            // EMPLOYEE → LOAN
            // =============================================
            modelBuilder.Entity<Loan>()
                .HasRequired(l => l.Employee)
                .WithMany(e => e.Loans)
                .HasForeignKey(l => l.EmployeeId)
                .WillCascadeOnDelete(false);

            // =============================================
            // LOAN → LOAN INSTALLMENT
            // =============================================
            modelBuilder.Entity<LoanInstallment>()
                .HasRequired(l => l.Loan)
                .WithMany(l => l.Installments)
                .HasForeignKey(l => l.LoanId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC → FABRIC BATCH
            // =============================================
            modelBuilder.Entity<FabricBatch>()
                .HasRequired(f => f.Fabric)
                .WithMany(f => f.Batches)
                .HasForeignKey(f => f.FabricId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SUPPLIER → FABRIC BATCH
            // =============================================
            modelBuilder.Entity<FabricBatch>()
                .HasOptional(f => f.Supplier)
                .WithMany(s => s.FabricBatches)
                .HasForeignKey(f => f.SupplierId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC → FABRIC STOCK
            // =============================================
            modelBuilder.Entity<FabricStock>()
                .HasRequired(f => f.Fabric)
                .WithMany(f => f.Stocks)
                .HasForeignKey(f => f.FabricId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC BATCH → FABRIC STOCK
            // =============================================
            modelBuilder.Entity<FabricStock>()
                .HasOptional(f => f.FabricBatch)
                .WithMany()
                .HasForeignKey(f => f.FabricBatchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → FABRIC STOCK
            // =============================================
            modelBuilder.Entity<FabricStock>()
                .HasRequired(f => f.Branch)
                .WithMany(b => b.FabricStocks)
                .HasForeignKey(f => f.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SUPPLIER → PURCHASE ORDER
            // =============================================
            modelBuilder.Entity<PurchaseOrder>()
                .HasRequired(p => p.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(p => p.SupplierId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → PURCHASE ORDER
            // =============================================
            modelBuilder.Entity<PurchaseOrder>()
                .HasRequired(p => p.Branch)
                .WithMany()
                .HasForeignKey(p => p.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // PURCHASE ORDER → PO DETAIL
            // =============================================
            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasRequired(p => p.PurchaseOrder)
                .WithMany(p => p.PurchaseOrderDetails)
                .HasForeignKey(p => p.PurchaseOrderId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC → PO DETAIL
            // =============================================
            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasRequired(p => p.Fabric)
                .WithMany()
                .HasForeignKey(p => p.FabricId)
                .WillCascadeOnDelete(false);

            // =============================================
            // CUSTOMER → SALES ORDER
            // =============================================
            modelBuilder.Entity<SalesOrder>()
                .HasRequired(s => s.Customer)
                .WithMany(c => c.SalesOrders)
                .HasForeignKey(s => s.CustomerId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → SALES ORDER (YEH ERROR KA SOURCE THA)
            // =============================================
            modelBuilder.Entity<SalesOrder>()
                .HasRequired(s => s.Branch)
                .WithMany()
                .HasForeignKey(s => s.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SALES ORDER → SO DETAIL
            // =============================================
            modelBuilder.Entity<SalesOrderDetail>()
                .HasRequired(s => s.SalesOrder)
                .WithMany(s => s.SalesOrderDetails)
                .HasForeignKey(s => s.SalesOrderId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC → SO DETAIL
            // =============================================
            modelBuilder.Entity<SalesOrderDetail>()
                .HasRequired(s => s.Fabric)
                .WithMany()
                .HasForeignKey(s => s.FabricId)
                .WillCascadeOnDelete(false);

            // =============================================
            // CUSTOMER → INVOICE
            // =============================================
            modelBuilder.Entity<Invoice>()
                .HasRequired(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SALES ORDER → INVOICE
            // =============================================
            modelBuilder.Entity<Invoice>()
                .HasOptional(i => i.SalesOrder)
                .WithMany(s => s.Invoices)
                .HasForeignKey(i => i.SalesOrderId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → INVOICE
            // =============================================
            modelBuilder.Entity<Invoice>()
                .HasRequired(i => i.Branch)
                .WithMany()
                .HasForeignKey(i => i.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SALES ORDER → DISPATCH
            // =============================================
            modelBuilder.Entity<Dispatch>()
                .HasRequired(d => d.SalesOrder)
                .WithMany(s => s.Dispatches)
                .HasForeignKey(d => d.SalesOrderId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → DISPATCH
            // =============================================
            modelBuilder.Entity<Dispatch>()
                .HasRequired(d => d.Branch)
                .WithMany()
                .HasForeignKey(d => d.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SALES ORDER → PRODUCTION PLAN
            // =============================================
            modelBuilder.Entity<ProductionPlan>()
                .HasRequired(p => p.SalesOrder)
                .WithMany()
                .HasForeignKey(p => p.SalesOrderId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → PRODUCTION PLAN
            // =============================================
            modelBuilder.Entity<ProductionPlan>()
                .HasRequired(p => p.Branch)
                .WithMany()
                .HasForeignKey(p => p.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // PRODUCTION PLAN → PRODUCTION ORDER
            // =============================================
            modelBuilder.Entity<ProductionOrder>()
                .HasRequired(p => p.ProductionPlan)
                .WithMany(p => p.ProductionOrders)
                .HasForeignKey(p => p.ProductionPlanId)
                .WillCascadeOnDelete(false);

            // =============================================
            // PRODUCTION ORDER → CUTTING
            // =============================================
            modelBuilder.Entity<Cutting>()
                .HasRequired(c => c.ProductionOrder)
                .WithMany()
                .HasForeignKey(c => c.ProductionOrderId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC → CUTTING
            // =============================================
            modelBuilder.Entity<Cutting>()
                .HasRequired(c => c.Fabric)
                .WithMany()
                .HasForeignKey(c => c.FabricId)
                .WillCascadeOnDelete(false);

            // =============================================
            // PRODUCTION ORDER → SEWING
            // =============================================
            modelBuilder.Entity<Sewing>()
                .HasRequired(s => s.ProductionOrder)
                .WithMany()
                .HasForeignKey(s => s.ProductionOrderId)
                .WillCascadeOnDelete(false);

            // =============================================
            // PRODUCTION ORDER → FINISHING
            // =============================================
            modelBuilder.Entity<Finishing>()
                .HasRequired(f => f.ProductionOrder)
                .WithMany()
                .HasForeignKey(f => f.ProductionOrderId)
                .WillCascadeOnDelete(false);

            // =============================================
            // NCR → CAPA
            // =============================================
            modelBuilder.Entity<CAPA>()
                .HasOptional(c => c.NCR)
                .WithMany(n => n.CAPAs)
                .HasForeignKey(c => c.NCRId)
                .WillCascadeOnDelete(false);

            // =============================================
            // QUALITY INSPECTION → NCR
            // =============================================
            modelBuilder.Entity<NCR>()
                .HasOptional(n => n.QualityInspection)
                .WithMany()
                .HasForeignKey(n => n.QualityInspectionId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → VOUCHER
            // =============================================
            modelBuilder.Entity<Voucher>()
                .HasRequired(v => v.Branch)
                .WithMany()
                .HasForeignKey(v => v.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // VOUCHER → VOUCHER DETAIL
            // =============================================
            modelBuilder.Entity<VoucherDetail>()
                .HasRequired(v => v.Voucher)
                .WithMany(v => v.VoucherDetails)
                .HasForeignKey(v => v.VoucherId)
                .WillCascadeOnDelete(false);

            // =============================================
            // ACCOUNT → VOUCHER DETAIL
            // =============================================
            modelBuilder.Entity<VoucherDetail>()
                .HasRequired(v => v.Account)
                .WithMany(a => a.VoucherDetails)
                .HasForeignKey(v => v.AccountId)
                .WillCascadeOnDelete(false);

            // =============================================
            // EMPLOYEE → EMPLOYEE DOCUMENT
            // =============================================
            modelBuilder.Entity<EmployeeDocument>()
                .HasRequired(e => e.Employee)
                .WithMany(e => e.Documents)
                .HasForeignKey(e => e.EmployeeId)
                .WillCascadeOnDelete(false);

            // =============================================
            // APPLICATION USER → BRANCH
            // =============================================
            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(u => u.Branch)
                .WithMany()
                .HasForeignKey(u => u.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // APPLICATION USER → EMPLOYEE
            // =============================================
            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(u => u.Employee)
                .WithMany()
                .HasForeignKey(u => u.EmployeeId)
                .WillCascadeOnDelete(false);
            // =============================================
            // FABRIC → GRN
            // =============================================
            modelBuilder.Entity<GRN>()
                .HasRequired(g => g.Fabric)
                .WithMany(f => f.GRNs)
                .HasForeignKey(g => g.FabricId)
                .WillCascadeOnDelete(false);

            // =============================================
            // SUPPLIER → GRN
            // =============================================
            modelBuilder.Entity<GRN>()
                .HasRequired(g => g.Supplier)
                .WithMany(s => s.GRNs)
                .HasForeignKey(g => g.SupplierId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → GRN
            // =============================================
            modelBuilder.Entity<GRN>()
                .HasRequired(g => g.Branch)
                .WithMany()
                .HasForeignKey(g => g.BranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // PURCHASE ORDER → GRN
            // =============================================
            modelBuilder.Entity<GRN>()
                .HasOptional(g => g.PurchaseOrder)
                .WithMany()
                .HasForeignKey(g => g.PurchaseOrderId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC BATCH → GRN
            // =============================================
            modelBuilder.Entity<GRN>()
                .HasOptional(g => g.FabricBatch)
                .WithMany()
                .HasForeignKey(g => g.FabricBatchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC → STOCK ISSUE
            // =============================================
            modelBuilder.Entity<StockIssue>()
                .HasRequired(s => s.Fabric)
                .WithMany(f => f.StockIssues)
                .HasForeignKey(s => s.FabricId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → STOCK ISSUE (FromBranch)
            // =============================================
            modelBuilder.Entity<StockIssue>()
                .HasRequired(s => s.FromBranch)
                .WithMany()
                .HasForeignKey(s => s.FromBranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC BATCH → STOCK ISSUE
            // =============================================
            modelBuilder.Entity<StockIssue>()
                .HasOptional(s => s.FabricBatch)
                .WithMany()
                .HasForeignKey(s => s.FabricBatchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC → STOCK TRANSFER
            // =============================================
            modelBuilder.Entity<StockTransfer>()
                .HasRequired(s => s.Fabric)
                .WithMany()
                .HasForeignKey(s => s.FabricId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC BATCH → STOCK TRANSFER
            // =============================================
            modelBuilder.Entity<StockTransfer>()
                .HasOptional(s => s.FabricBatch)
                .WithMany()
                .HasForeignKey(s => s.FabricBatchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → STOCK TRANSFER (FromBranch)
            // =============================================
            modelBuilder.Entity<StockTransfer>()
                .HasRequired(s => s.FromBranch)
                .WithMany()
                .HasForeignKey(s => s.FromBranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // BRANCH → STOCK TRANSFER (ToBranch)
            // =============================================
            modelBuilder.Entity<StockTransfer>()
                .HasRequired(s => s.ToBranch)
                .WithMany()
                .HasForeignKey(s => s.ToBranchId)
                .WillCascadeOnDelete(false);

            // =============================================
            // FABRIC → QUALITY INSPECTION
            // =============================================
            modelBuilder.Entity<QualityInspection>()
                .HasRequired(q => q.Fabric)        // ← HasOptional ki jagah HasRequired
                .WithMany()
                .HasForeignKey(q => q.FabricId)
                .WillCascadeOnDelete(false);

            // =============================================
            // GRN → QUALITY INSPECTION
            // =============================================
            modelBuilder.Entity<QualityInspection>()
                .HasOptional(q => q.GRN)
                .WithMany()
                .HasForeignKey(q => q.GRNId)
                .WillCascadeOnDelete(false);

            // =============================================
            // PRODUCTION ORDER → QUALITY INSPECTION
            // =============================================
            modelBuilder.Entity<QualityInspection>()
                .HasOptional(q => q.ProductionOrder)
                .WithMany()
                .HasForeignKey(q => q.ProductionOrderId)
                .WillCascadeOnDelete(false);
        }
    }
}