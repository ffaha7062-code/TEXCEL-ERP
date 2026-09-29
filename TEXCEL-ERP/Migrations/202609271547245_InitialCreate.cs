namespace TEXCEL_ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        AccountCode = c.String(nullable: false, maxLength: 20),
                        AccountName = c.String(nullable: false, maxLength: 200),
                        AccountType = c.String(nullable: false, maxLength: 50),
                        ParentAccountId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        OpeningBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AccountId)
                .ForeignKey("dbo.Accounts", t => t.ParentAccountId)
                .Index(t => t.ParentAccountId);
            
            CreateTable(
                "dbo.VoucherDetails",
                c => new
                    {
                        VoucherDetailId = c.Int(nullable: false, identity: true),
                        VoucherId = c.Int(nullable: false),
                        AccountId = c.Int(nullable: false),
                        DebitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.VoucherDetailId)
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .ForeignKey("dbo.Vouchers", t => t.VoucherId)
                .Index(t => t.VoucherId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Vouchers",
                c => new
                    {
                        VoucherId = c.Int(nullable: false, identity: true),
                        VoucherNumber = c.String(nullable: false, maxLength: 50),
                        VoucherType = c.String(nullable: false, maxLength: 20),
                        VoucherDate = c.DateTime(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Narration = c.String(maxLength: 1000),
                        TotalDebit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCredit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovedById = c.Int(),
                        ApprovedDate = c.DateTime(),
                        CreatedById = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VoucherId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        BranchId = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        BranchCode = c.String(nullable: false, maxLength: 20),
                        BranchName = c.String(nullable: false, maxLength: 200),
                        Address = c.String(maxLength: 500),
                        City = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Email = c.String(maxLength: 100),
                        ManagerName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BranchId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId)
                .Index(t => t.BranchCode, unique: true);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 200),
                        LegalName = c.String(maxLength: 100),
                        NTN = c.String(maxLength: 50),
                        GSTNumber = c.String(maxLength: 50),
                        Address = c.String(maxLength: 500),
                        City = c.String(maxLength: 100),
                        Country = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Email = c.String(maxLength: 100),
                        Website = c.String(maxLength: 200),
                        LogoPath = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        EmployeeCode = c.String(nullable: false, maxLength: 20),
                        CompanyId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        DesignationId = c.Int(nullable: false),
                        ShiftId = c.Int(),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        FatherHusbandName = c.String(maxLength: 100),
                        CNIC = c.String(nullable: false, maxLength: 20),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        Nationality = c.String(maxLength: 100),
                        Religion = c.String(maxLength: 50),
                        MaritalStatus = c.String(maxLength: 20),
                        BloodGroup = c.String(maxLength: 20),
                        MobileNumber = c.String(nullable: false, maxLength: 20),
                        EmergencyContact = c.String(maxLength: 20),
                        Email = c.String(maxLength: 100),
                        CurrentAddress = c.String(maxLength: 500),
                        PermanentAddress = c.String(maxLength: 500),
                        JoiningDate = c.DateTime(nullable: false),
                        ConfirmationDate = c.DateTime(),
                        ResignationDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        BasicSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BankAccountNumber = c.String(maxLength: 50),
                        BankName = c.String(maxLength: 100),
                        BankBranchName = c.String(maxLength: 100),
                        PhotoPath = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Designations", t => t.DesignationId)
                .ForeignKey("dbo.Shifts", t => t.ShiftId)
                .Index(t => t.EmployeeCode, unique: true)
                .Index(t => t.CompanyId)
                .Index(t => t.BranchId)
                .Index(t => t.DepartmentId)
                .Index(t => t.DesignationId)
                .Index(t => t.ShiftId)
                .Index(t => t.CNIC, unique: true);
            
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        AttendanceId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        AttendanceDate = c.DateTime(nullable: false),
                        ShiftId = c.Int(),
                        CheckIn = c.Time(precision: 7),
                        CheckOut = c.Time(precision: 7),
                        Status = c.Int(nullable: false),
                        LateMinutes = c.Int(nullable: false),
                        EarlyGoingMinutes = c.Int(nullable: false),
                        OvertimeHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WorkedHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(maxLength: 500),
                        IsApproved = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AttendanceId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .ForeignKey("dbo.Shifts", t => t.ShiftId)
                .Index(t => t.EmployeeId)
                .Index(t => t.ShiftId);
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        ShiftId = c.Int(nullable: false, identity: true),
                        ShiftName = c.String(nullable: false, maxLength: 50),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        BreakDuration = c.Int(nullable: false),
                        OvertimeStartAfter = c.Int(nullable: false),
                        OvertimeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LateAllowance = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ShiftId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        DepartmentCode = c.String(nullable: false, maxLength: 20),
                        DepartmentName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.BranchId)
                .Index(t => t.DepartmentCode, unique: true);
            
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        DesignationId = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        DesignationCode = c.String(nullable: false, maxLength: 20),
                        DesignationName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        Grade = c.String(maxLength: 10),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DesignationId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.DepartmentId)
                .Index(t => t.DesignationCode, unique: true);
            
            CreateTable(
                "dbo.EmployeeDocuments",
                c => new
                    {
                        EmployeeDocumentId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        DocumentType = c.String(nullable: false, maxLength: 100),
                        DocumentName = c.String(nullable: false, maxLength: 200),
                        FilePath = c.String(nullable: false, maxLength: 500),
                        FileSizeKB = c.Long(),
                        ContentType = c.String(maxLength: 100),
                        ExpiryDate = c.DateTime(),
                        UploadedDate = c.DateTime(nullable: false),
                        UploadedById = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeDocumentId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Leaves",
                c => new
                    {
                        LeaveId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        LeaveType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        TotalDays = c.Int(nullable: false),
                        Reason = c.String(nullable: false, maxLength: 500),
                        Status = c.Int(nullable: false),
                        ApprovedById = c.Int(),
                        ApprovedDate = c.DateTime(),
                        ApprovalRemarks = c.String(maxLength: 500),
                        AttachmentPath = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LeaveId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        LoanId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        LoanType = c.String(nullable: false, maxLength: 50),
                        LoanAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InterestRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TenureMonths = c.Int(nullable: false),
                        EMIAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPayable = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OutstandingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Reason = c.String(maxLength: 500),
                        Status = c.String(maxLength: 20),
                        ApprovedById = c.Int(),
                        ApprovedDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.LoanInstallments",
                c => new
                    {
                        LoanInstallmentId = c.Int(nullable: false, identity: true),
                        LoanId = c.Int(nullable: false),
                        InstallmentNumber = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        EMIAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrincipalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InterestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaidAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaidDate = c.DateTime(),
                        Status = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LoanInstallmentId)
                .ForeignKey("dbo.Loans", t => t.LoanId)
                .Index(t => t.LoanId);
            
            CreateTable(
                "dbo.Payrolls",
                c => new
                    {
                        PayrollId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        BasicSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalEarnings = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDeductions = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PresentDays = c.Int(nullable: false),
                        AbsentDays = c.Int(nullable: false),
                        LeaveDays = c.Int(nullable: false),
                        OvertimeHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OvertimeAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        ProcessedDate = c.DateTime(),
                        ProcessedById = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PayrollId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.PayrollDetails",
                c => new
                    {
                        PayrollDetailId = c.Int(nullable: false, identity: true),
                        PayrollId = c.Int(nullable: false),
                        SalaryHeadId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PayrollDetailId)
                .ForeignKey("dbo.Payrolls", t => t.PayrollId)
                .ForeignKey("dbo.SalaryHeads", t => t.SalaryHeadId)
                .Index(t => t.PayrollId)
                .Index(t => t.SalaryHeadId);
            
            CreateTable(
                "dbo.SalaryHeads",
                c => new
                    {
                        SalaryHeadId = c.Int(nullable: false, identity: true),
                        HeadName = c.String(nullable: false, maxLength: 100),
                        HeadType = c.String(nullable: false, maxLength: 20),
                        CalculationType = c.String(maxLength: 50),
                        DefaultValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsTaxable = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SalaryHeadId);
            
            CreateTable(
                "dbo.SalaryStructures",
                c => new
                    {
                        SalaryStructureId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        SalaryHeadId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Percentage = c.Decimal(precision: 18, scale: 2),
                        EffectiveFrom = c.DateTime(nullable: false),
                        EffectiveTo = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SalaryStructureId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .ForeignKey("dbo.SalaryHeads", t => t.SalaryHeadId)
                .Index(t => t.EmployeeId)
                .Index(t => t.SalaryHeadId);
            
            CreateTable(
                "dbo.FabricStocks",
                c => new
                    {
                        FabricStockId = c.Int(nullable: false, identity: true),
                        FabricId = c.Int(nullable: false),
                        FabricBatchId = c.Int(),
                        BranchId = c.Int(nullable: false),
                        Warehouse = c.String(maxLength: 100),
                        Location = c.String(maxLength: 100),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ReservedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UOM = c.String(maxLength: 20),
                        RatePerUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FabricStockId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Fabrics", t => t.FabricId)
                .ForeignKey("dbo.FabricBatches", t => t.FabricBatchId)
                .Index(t => t.FabricId)
                .Index(t => t.FabricBatchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Fabrics",
                c => new
                    {
                        FabricId = c.Int(nullable: false, identity: true),
                        FabricCode = c.String(nullable: false, maxLength: 20),
                        FabricType = c.String(nullable: false, maxLength: 100),
                        Composition = c.String(maxLength: 50),
                        Weave = c.String(maxLength: 50),
                        GSM = c.Int(),
                        Width = c.Decimal(precision: 18, scale: 2),
                        Color = c.String(maxLength: 50),
                        ShadeCode = c.String(maxLength: 50),
                        Design = c.String(maxLength: 50),
                        Grade = c.Int(nullable: false),
                        UOM = c.String(maxLength: 20),
                        RatePerUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinimumStock = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FabricId)
                .Index(t => t.FabricCode, unique: true);
            
            CreateTable(
                "dbo.FabricBatches",
                c => new
                    {
                        FabricBatchId = c.Int(nullable: false, identity: true),
                        FabricId = c.Int(nullable: false),
                        BatchNumber = c.String(nullable: false, maxLength: 50),
                        SupplierId = c.Int(),
                        PurchaseDate = c.DateTime(),
                        ExpiryDate = c.DateTime(),
                        DyeLotNumber = c.String(maxLength: 50),
                        QualityCertificatePath = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FabricBatchId)
                .ForeignKey("dbo.Fabrics", t => t.FabricId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.FabricId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierId = c.Int(nullable: false, identity: true),
                        SupplierCode = c.String(nullable: false, maxLength: 20),
                        SupplierName = c.String(nullable: false, maxLength: 200),
                        ContactPerson = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Email = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        City = c.String(maxLength: 100),
                        NTN = c.String(maxLength: 50),
                        GSTNumber = c.String(maxLength: 50),
                        BankName = c.String(maxLength: 100),
                        BankAccountNumber = c.String(maxLength: 50),
                        PaymentTerms = c.String(maxLength: 100),
                        CreditLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditDays = c.Int(nullable: false),
                        LeadTimeDays = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SupplierId)
                .Index(t => t.SupplierCode, unique: true);
            
            CreateTable(
                "dbo.GRNs",
                c => new
                    {
                        GRNId = c.Int(nullable: false, identity: true),
                        GRNNumber = c.String(nullable: false, maxLength: 50),
                        PurchaseOrderId = c.Int(),
                        SupplierId = c.Int(nullable: false),
                        FabricId = c.Int(nullable: false),
                        FabricBatchId = c.Int(),
                        BranchId = c.Int(nullable: false),
                        ReceiptDate = c.DateTime(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        UOM = c.String(maxLength: 20),
                        RatePerUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RollCount = c.Int(),
                        QualityStatus = c.String(maxLength: 20),
                        QCRemarks = c.String(maxLength: 500),
                        Warehouse = c.String(maxLength: 100),
                        Location = c.String(maxLength: 100),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovedById = c.Int(),
                        ApprovedDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GRNId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Fabrics", t => t.FabricId)
                .ForeignKey("dbo.FabricBatches", t => t.FabricBatchId)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.SupplierId)
                .Index(t => t.FabricId)
                .Index(t => t.FabricBatchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        PurchaseOrderId = c.Int(nullable: false, identity: true),
                        PONumber = c.String(nullable: false, maxLength: 50),
                        SupplierId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        PODate = c.DateTime(nullable: false),
                        ExpectedDeliveryDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(maxLength: 500),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovedById = c.Int(),
                        ApprovedDate = c.DateTime(),
                        CreatedById = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.SupplierId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.PurchaseOrderDetails",
                c => new
                    {
                        PurchaseOrderDetailId = c.Int(nullable: false, identity: true),
                        PurchaseOrderId = c.Int(nullable: false),
                        FabricId = c.Int(nullable: false),
                        FabricBatchId = c.Int(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UOM = c.String(maxLength: 20),
                        RatePerUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceivedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderDetailId)
                .ForeignKey("dbo.Fabrics", t => t.FabricId)
                .ForeignKey("dbo.FabricBatches", t => t.FabricBatchId)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderId)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.FabricId)
                .Index(t => t.FabricBatchId);
            
            CreateTable(
                "dbo.StockIssues",
                c => new
                    {
                        StockIssueId = c.Int(nullable: false, identity: true),
                        IssueNumber = c.String(nullable: false, maxLength: 50),
                        FabricId = c.Int(nullable: false),
                        FabricBatchId = c.Int(),
                        FromBranchId = c.Int(nullable: false),
                        ToDepartment = c.String(maxLength: 100),
                        Purpose = c.String(maxLength: 200),
                        Reference = c.String(maxLength: 100),
                        IssueDate = c.DateTime(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        UOM = c.String(maxLength: 20),
                        IssuedById = c.Int(),
                        ReceivedBy = c.String(maxLength: 100),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovedById = c.Int(),
                        ApprovedDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StockIssueId)
                .ForeignKey("dbo.Fabrics", t => t.FabricId)
                .ForeignKey("dbo.FabricBatches", t => t.FabricBatchId)
                .ForeignKey("dbo.Branches", t => t.FromBranchId)
                .Index(t => t.FabricId)
                .Index(t => t.FabricBatchId)
                .Index(t => t.FromBranchId);
            
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        AuditLogId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        UserName = c.String(maxLength: 100),
                        BranchId = c.Int(),
                        ActionType = c.String(maxLength: 50),
                        Module = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        IPAddress = c.String(maxLength: 50),
                        OldValue = c.String(maxLength: 500),
                        NewValue = c.String(maxLength: 500),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AuditLogId);
            
            CreateTable(
                "dbo.CAPAs",
                c => new
                    {
                        CAPAId = c.Int(nullable: false, identity: true),
                        CAPANumber = c.String(nullable: false, maxLength: 50),
                        NCRId = c.Int(),
                        CAPADate = c.DateTime(nullable: false),
                        ActionType = c.String(maxLength: 50),
                        ActionDescription = c.String(maxLength: 1000),
                        ResponsiblePerson = c.String(maxLength: 100),
                        TargetDate = c.DateTime(),
                        CompletionDate = c.DateTime(),
                        Verification = c.String(maxLength: 500),
                        Status = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CAPAId)
                .ForeignKey("dbo.NCRs", t => t.NCRId)
                .Index(t => t.NCRId);
            
            CreateTable(
                "dbo.NCRs",
                c => new
                    {
                        NCRId = c.Int(nullable: false, identity: true),
                        NCRNumber = c.String(nullable: false, maxLength: 50),
                        QualityInspectionId = c.Int(),
                        NCRDate = c.DateTime(nullable: false),
                        IssueDescription = c.String(maxLength: 1000),
                        RootCause = c.String(maxLength: 1000),
                        CorrectiveAction = c.String(maxLength: 1000),
                        PreventiveAction = c.String(maxLength: 1000),
                        ResponsiblePerson = c.String(maxLength: 100),
                        TargetDate = c.DateTime(),
                        ClosureDate = c.DateTime(),
                        Status = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NCRId)
                .ForeignKey("dbo.QualityInspections", t => t.QualityInspectionId)
                .Index(t => t.QualityInspectionId);
            
            CreateTable(
                "dbo.QualityInspections",
                c => new
                    {
                        QualityInspectionId = c.Int(nullable: false, identity: true),
                        InspectionNumber = c.String(nullable: false, maxLength: 50),
                        GRNId = c.Int(),
                        ProductionOrderId = c.Int(),
                        FabricId = c.Int(nullable: false),
                        InspectionDate = c.DateTime(nullable: false),
                        Inspector = c.String(maxLength: 100),
                        InspectedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PassedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FailedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReworkQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DefectType = c.String(maxLength: 100),
                        DefectDescription = c.String(maxLength: 500),
                        DefectImagePath = c.String(maxLength: 500),
                        RejectionPercentage = c.Decimal(precision: 18, scale: 2),
                        Grade = c.Int(),
                        Remarks = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QualityInspectionId)
                .ForeignKey("dbo.Fabrics", t => t.FabricId)
                .ForeignKey("dbo.GRNs", t => t.GRNId)
                .ForeignKey("dbo.ProductionOrders", t => t.ProductionOrderId)
                .Index(t => t.GRNId)
                .Index(t => t.ProductionOrderId)
                .Index(t => t.FabricId);
            
            CreateTable(
                "dbo.ProductionOrders",
                c => new
                    {
                        ProductionOrderId = c.Int(nullable: false, identity: true),
                        OrderNumber = c.String(nullable: false, maxLength: 50),
                        ProductionPlanId = c.Int(nullable: false),
                        Stage = c.String(maxLength: 50),
                        LineNumber = c.String(maxLength: 50),
                        Supervisor = c.String(maxLength: 100),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        PlannedQuantity = c.Int(nullable: false),
                        ProducedQuantity = c.Int(nullable: false),
                        RejectedQuantity = c.Int(nullable: false),
                        Status = c.String(maxLength: 20),
                        Efficiency = c.Decimal(precision: 18, scale: 2),
                        Remarks = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProductionOrderId)
                .ForeignKey("dbo.ProductionPlans", t => t.ProductionPlanId)
                .Index(t => t.ProductionPlanId);
            
            CreateTable(
                "dbo.ProductionPlans",
                c => new
                    {
                        ProductionPlanId = c.Int(nullable: false, identity: true),
                        PlanNumber = c.String(nullable: false, maxLength: 50),
                        SalesOrderId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Style = c.String(maxLength: 100),
                        PlanDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        PlannedQuantity = c.Int(nullable: false),
                        ProducedQuantity = c.Int(nullable: false),
                        Status = c.String(maxLength: 20),
                        Priority = c.String(maxLength: 20),
                        Remarks = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProductionPlanId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.SalesOrders", t => t.SalesOrderId)
                .Index(t => t.SalesOrderId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.SalesOrders",
                c => new
                    {
                        SalesOrderId = c.Int(nullable: false, identity: true),
                        SONumber = c.String(nullable: false, maxLength: 50),
                        CustomerId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        DeliveryDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        OrderType = c.String(maxLength: 50),
                        Currency = c.String(maxLength: 10),
                        SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FreightAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentTerms = c.String(maxLength: 100),
                        ShippingMark = c.String(maxLength: 500),
                        Remarks = c.String(maxLength: 500),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovedById = c.Int(),
                        ApprovedDate = c.DateTime(),
                        CreatedById = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SalesOrderId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        CustomerCode = c.String(nullable: false, maxLength: 20),
                        CustomerName = c.String(nullable: false, maxLength: 200),
                        ContactPerson = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Email = c.String(maxLength: 100),
                        BillingAddress = c.String(maxLength: 500),
                        ShippingAddress = c.String(maxLength: 500),
                        City = c.String(maxLength: 100),
                        NTN = c.String(maxLength: 50),
                        GSTNumber = c.String(maxLength: 50),
                        CustomerType = c.String(maxLength: 50),
                        CreditLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditDays = c.Int(nullable: false),
                        PaymentTerms = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId)
                .Index(t => t.CustomerCode, unique: true);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.String(nullable: false, maxLength: 50),
                        CustomerId = c.Int(nullable: false),
                        SalesOrderId = c.Int(),
                        BranchId = c.Int(nullable: false),
                        InvoiceDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(),
                        SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FreightAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaidAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentStatus = c.Int(nullable: false),
                        PaymentDate = c.DateTime(),
                        Remarks = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.SalesOrders", t => t.SalesOrderId)
                .Index(t => t.CustomerId)
                .Index(t => t.SalesOrderId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Dispatches",
                c => new
                    {
                        DispatchId = c.Int(nullable: false, identity: true),
                        DispatchNumber = c.String(nullable: false, maxLength: 50),
                        SalesOrderId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        DispatchDate = c.DateTime(nullable: false),
                        VehicleNumber = c.String(maxLength: 50),
                        DriverName = c.String(maxLength: 100),
                        DriverContact = c.String(maxLength: 20),
                        ExpectedDeliveryDate = c.DateTime(),
                        DeliveredDate = c.DateTime(),
                        Status = c.String(maxLength: 20),
                        TotalCartons = c.Int(),
                        TotalQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippingAddress = c.String(maxLength: 500),
                        Remarks = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DispatchId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.SalesOrders", t => t.SalesOrderId)
                .Index(t => t.SalesOrderId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.SalesOrderDetails",
                c => new
                    {
                        SalesOrderDetailId = c.Int(nullable: false, identity: true),
                        SalesOrderId = c.Int(nullable: false),
                        FabricId = c.Int(nullable: false),
                        FabricBatchId = c.Int(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UOM = c.String(maxLength: 20),
                        RatePerUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DispatchedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SalesOrderDetailId)
                .ForeignKey("dbo.Fabrics", t => t.FabricId)
                .ForeignKey("dbo.FabricBatches", t => t.FabricBatchId)
                .ForeignKey("dbo.SalesOrders", t => t.SalesOrderId)
                .Index(t => t.SalesOrderId)
                .Index(t => t.FabricId)
                .Index(t => t.FabricBatchId);
            
            CreateTable(
                "dbo.Cuttings",
                c => new
                    {
                        CuttingId = c.Int(nullable: false, identity: true),
                        ProductionOrderId = c.Int(nullable: false),
                        FabricId = c.Int(nullable: false),
                        FabricBatchId = c.Int(),
                        CuttingDate = c.DateTime(nullable: false),
                        MarkerPlan = c.String(maxLength: 200),
                        MarkerEfficiency = c.Decimal(precision: 18, scale: 2),
                        FabricConsumed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CutPieces = c.Int(nullable: false),
                        DamagedPieces = c.Int(nullable: false),
                        WastagePercentage = c.Decimal(precision: 18, scale: 2),
                        Operator = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CuttingId)
                .ForeignKey("dbo.Fabrics", t => t.FabricId)
                .ForeignKey("dbo.FabricBatches", t => t.FabricBatchId)
                .ForeignKey("dbo.ProductionOrders", t => t.ProductionOrderId)
                .Index(t => t.ProductionOrderId)
                .Index(t => t.FabricId)
                .Index(t => t.FabricBatchId);
            
            CreateTable(
                "dbo.Finishings",
                c => new
                    {
                        FinishingId = c.Int(nullable: false, identity: true),
                        ProductionOrderId = c.Int(nullable: false),
                        FinishingDate = c.DateTime(nullable: false),
                        Operation = c.String(maxLength: 100),
                        Operator = c.String(maxLength: 100),
                        InputPieces = c.Int(nullable: false),
                        OutputPieces = c.Int(nullable: false),
                        RejectedPieces = c.Int(nullable: false),
                        ReworkPieces = c.Int(nullable: false),
                        Efficiency = c.Decimal(precision: 18, scale: 2),
                        CartonsPacked = c.Int(),
                        Remarks = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FinishingId)
                .ForeignKey("dbo.ProductionOrders", t => t.ProductionOrderId)
                .Index(t => t.ProductionOrderId);
            
            CreateTable(
                "dbo.LeavePolicies",
                c => new
                    {
                        LeavePolicyId = c.Int(nullable: false, identity: true),
                        LeaveType = c.Int(nullable: false),
                        TotalLeaves = c.Int(nullable: false),
                        MaxCarryForward = c.Int(nullable: false),
                        IsEncashable = c.Boolean(nullable: false),
                        RequiresAttachment = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LeavePolicyId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 200),
                        Message = c.String(nullable: false, maxLength: 1000),
                        Type = c.String(maxLength: 50),
                        Module = c.String(maxLength: 50),
                        ReferenceId = c.Int(),
                        IsRead = c.Boolean(nullable: false),
                        ReadDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Sewings",
                c => new
                    {
                        SewingId = c.Int(nullable: false, identity: true),
                        ProductionOrderId = c.Int(nullable: false),
                        SewingDate = c.DateTime(nullable: false),
                        LineNumber = c.String(maxLength: 50),
                        Operator = c.String(maxLength: 100),
                        Operation = c.String(maxLength: 100),
                        SMV = c.Decimal(precision: 18, scale: 2),
                        ProducedPieces = c.Int(nullable: false),
                        RejectedPieces = c.Int(nullable: false),
                        Efficiency = c.Decimal(precision: 18, scale: 2),
                        DowntimeMinutes = c.Int(nullable: false),
                        DowntimeReason = c.String(maxLength: 200),
                        Remarks = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SewingId)
                .ForeignKey("dbo.ProductionOrders", t => t.ProductionOrderId)
                .Index(t => t.ProductionOrderId);
            
            CreateTable(
                "dbo.StockTransfers",
                c => new
                    {
                        StockTransferId = c.Int(nullable: false, identity: true),
                        TransferNumber = c.String(nullable: false, maxLength: 50),
                        FabricId = c.Int(nullable: false),
                        FabricBatchId = c.Int(),
                        FromBranchId = c.Int(nullable: false),
                        ToBranchId = c.Int(nullable: false),
                        TransferDate = c.DateTime(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        UOM = c.String(maxLength: 20),
                        VehicleNumber = c.String(maxLength: 50),
                        DriverName = c.String(maxLength: 100),
                        ExpectedDeliveryDate = c.DateTime(),
                        Status = c.String(maxLength: 20),
                        ReceivedDate = c.DateTime(),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovedById = c.Int(),
                        ApprovedDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StockTransferId)
                .ForeignKey("dbo.Fabrics", t => t.FabricId)
                .ForeignKey("dbo.FabricBatches", t => t.FabricBatchId)
                .ForeignKey("dbo.Branches", t => t.FromBranchId)
                .ForeignKey("dbo.Branches", t => t.ToBranchId)
                .Index(t => t.FabricId)
                .Index(t => t.FabricBatchId)
                .Index(t => t.FromBranchId)
                .Index(t => t.ToBranchId);
            
            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        SystemSettingId = c.Int(nullable: false, identity: true),
                        SettingKey = c.String(nullable: false, maxLength: 100),
                        SettingValue = c.String(nullable: false, maxLength: 1000),
                        Description = c.String(maxLength: 500),
                        Category = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SystemSettingId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        BranchId = c.Int(),
                        EmployeeId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        ProfilePicture = c.String(maxLength: 500),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.BranchId)
                .Index(t => t.EmployeeId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.StockTransfers", "ToBranchId", "dbo.Branches");
            DropForeignKey("dbo.StockTransfers", "FromBranchId", "dbo.Branches");
            DropForeignKey("dbo.StockTransfers", "FabricBatchId", "dbo.FabricBatches");
            DropForeignKey("dbo.StockTransfers", "FabricId", "dbo.Fabrics");
            DropForeignKey("dbo.Sewings", "ProductionOrderId", "dbo.ProductionOrders");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Finishings", "ProductionOrderId", "dbo.ProductionOrders");
            DropForeignKey("dbo.Cuttings", "ProductionOrderId", "dbo.ProductionOrders");
            DropForeignKey("dbo.Cuttings", "FabricBatchId", "dbo.FabricBatches");
            DropForeignKey("dbo.Cuttings", "FabricId", "dbo.Fabrics");
            DropForeignKey("dbo.CAPAs", "NCRId", "dbo.NCRs");
            DropForeignKey("dbo.NCRs", "QualityInspectionId", "dbo.QualityInspections");
            DropForeignKey("dbo.QualityInspections", "ProductionOrderId", "dbo.ProductionOrders");
            DropForeignKey("dbo.ProductionOrders", "ProductionPlanId", "dbo.ProductionPlans");
            DropForeignKey("dbo.ProductionPlans", "SalesOrderId", "dbo.SalesOrders");
            DropForeignKey("dbo.SalesOrderDetails", "SalesOrderId", "dbo.SalesOrders");
            DropForeignKey("dbo.SalesOrderDetails", "FabricBatchId", "dbo.FabricBatches");
            DropForeignKey("dbo.SalesOrderDetails", "FabricId", "dbo.Fabrics");
            DropForeignKey("dbo.Dispatches", "SalesOrderId", "dbo.SalesOrders");
            DropForeignKey("dbo.Dispatches", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.SalesOrders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Invoices", "SalesOrderId", "dbo.SalesOrders");
            DropForeignKey("dbo.Invoices", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Invoices", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.SalesOrders", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.ProductionPlans", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.QualityInspections", "GRNId", "dbo.GRNs");
            DropForeignKey("dbo.QualityInspections", "FabricId", "dbo.Fabrics");
            DropForeignKey("dbo.VoucherDetails", "VoucherId", "dbo.Vouchers");
            DropForeignKey("dbo.Vouchers", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.FabricStocks", "FabricBatchId", "dbo.FabricBatches");
            DropForeignKey("dbo.FabricStocks", "FabricId", "dbo.Fabrics");
            DropForeignKey("dbo.StockIssues", "FromBranchId", "dbo.Branches");
            DropForeignKey("dbo.StockIssues", "FabricBatchId", "dbo.FabricBatches");
            DropForeignKey("dbo.StockIssues", "FabricId", "dbo.Fabrics");
            DropForeignKey("dbo.FabricBatches", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.GRNs", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.GRNs", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrders", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseOrderDetails", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrderDetails", "FabricBatchId", "dbo.FabricBatches");
            DropForeignKey("dbo.PurchaseOrderDetails", "FabricId", "dbo.Fabrics");
            DropForeignKey("dbo.PurchaseOrders", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.GRNs", "FabricBatchId", "dbo.FabricBatches");
            DropForeignKey("dbo.GRNs", "FabricId", "dbo.Fabrics");
            DropForeignKey("dbo.GRNs", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.FabricBatches", "FabricId", "dbo.Fabrics");
            DropForeignKey("dbo.FabricStocks", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Branches", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Employees", "ShiftId", "dbo.Shifts");
            DropForeignKey("dbo.PayrollDetails", "SalaryHeadId", "dbo.SalaryHeads");
            DropForeignKey("dbo.SalaryStructures", "SalaryHeadId", "dbo.SalaryHeads");
            DropForeignKey("dbo.SalaryStructures", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.PayrollDetails", "PayrollId", "dbo.Payrolls");
            DropForeignKey("dbo.Payrolls", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.LoanInstallments", "LoanId", "dbo.Loans");
            DropForeignKey("dbo.Loans", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Leaves", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeDocuments", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "DesignationId", "dbo.Designations");
            DropForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Designations", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Employees", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Attendances", "ShiftId", "dbo.Shifts");
            DropForeignKey("dbo.Attendances", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.VoucherDetails", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "ParentAccountId", "dbo.Accounts");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeId" });
            DropIndex("dbo.AspNetUsers", new[] { "BranchId" });
            DropIndex("dbo.StockTransfers", new[] { "ToBranchId" });
            DropIndex("dbo.StockTransfers", new[] { "FromBranchId" });
            DropIndex("dbo.StockTransfers", new[] { "FabricBatchId" });
            DropIndex("dbo.StockTransfers", new[] { "FabricId" });
            DropIndex("dbo.Sewings", new[] { "ProductionOrderId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Finishings", new[] { "ProductionOrderId" });
            DropIndex("dbo.Cuttings", new[] { "FabricBatchId" });
            DropIndex("dbo.Cuttings", new[] { "FabricId" });
            DropIndex("dbo.Cuttings", new[] { "ProductionOrderId" });
            DropIndex("dbo.SalesOrderDetails", new[] { "FabricBatchId" });
            DropIndex("dbo.SalesOrderDetails", new[] { "FabricId" });
            DropIndex("dbo.SalesOrderDetails", new[] { "SalesOrderId" });
            DropIndex("dbo.Dispatches", new[] { "BranchId" });
            DropIndex("dbo.Dispatches", new[] { "SalesOrderId" });
            DropIndex("dbo.Invoices", new[] { "BranchId" });
            DropIndex("dbo.Invoices", new[] { "SalesOrderId" });
            DropIndex("dbo.Invoices", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "CustomerCode" });
            DropIndex("dbo.SalesOrders", new[] { "BranchId" });
            DropIndex("dbo.SalesOrders", new[] { "CustomerId" });
            DropIndex("dbo.ProductionPlans", new[] { "BranchId" });
            DropIndex("dbo.ProductionPlans", new[] { "SalesOrderId" });
            DropIndex("dbo.ProductionOrders", new[] { "ProductionPlanId" });
            DropIndex("dbo.QualityInspections", new[] { "FabricId" });
            DropIndex("dbo.QualityInspections", new[] { "ProductionOrderId" });
            DropIndex("dbo.QualityInspections", new[] { "GRNId" });
            DropIndex("dbo.NCRs", new[] { "QualityInspectionId" });
            DropIndex("dbo.CAPAs", new[] { "NCRId" });
            DropIndex("dbo.StockIssues", new[] { "FromBranchId" });
            DropIndex("dbo.StockIssues", new[] { "FabricBatchId" });
            DropIndex("dbo.StockIssues", new[] { "FabricId" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "FabricBatchId" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "FabricId" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "PurchaseOrderId" });
            DropIndex("dbo.PurchaseOrders", new[] { "BranchId" });
            DropIndex("dbo.PurchaseOrders", new[] { "SupplierId" });
            DropIndex("dbo.GRNs", new[] { "BranchId" });
            DropIndex("dbo.GRNs", new[] { "FabricBatchId" });
            DropIndex("dbo.GRNs", new[] { "FabricId" });
            DropIndex("dbo.GRNs", new[] { "SupplierId" });
            DropIndex("dbo.GRNs", new[] { "PurchaseOrderId" });
            DropIndex("dbo.Suppliers", new[] { "SupplierCode" });
            DropIndex("dbo.FabricBatches", new[] { "SupplierId" });
            DropIndex("dbo.FabricBatches", new[] { "FabricId" });
            DropIndex("dbo.Fabrics", new[] { "FabricCode" });
            DropIndex("dbo.FabricStocks", new[] { "BranchId" });
            DropIndex("dbo.FabricStocks", new[] { "FabricBatchId" });
            DropIndex("dbo.FabricStocks", new[] { "FabricId" });
            DropIndex("dbo.SalaryStructures", new[] { "SalaryHeadId" });
            DropIndex("dbo.SalaryStructures", new[] { "EmployeeId" });
            DropIndex("dbo.PayrollDetails", new[] { "SalaryHeadId" });
            DropIndex("dbo.PayrollDetails", new[] { "PayrollId" });
            DropIndex("dbo.Payrolls", new[] { "EmployeeId" });
            DropIndex("dbo.LoanInstallments", new[] { "LoanId" });
            DropIndex("dbo.Loans", new[] { "EmployeeId" });
            DropIndex("dbo.Leaves", new[] { "EmployeeId" });
            DropIndex("dbo.EmployeeDocuments", new[] { "EmployeeId" });
            DropIndex("dbo.Designations", new[] { "DesignationCode" });
            DropIndex("dbo.Designations", new[] { "DepartmentId" });
            DropIndex("dbo.Departments", new[] { "DepartmentCode" });
            DropIndex("dbo.Departments", new[] { "BranchId" });
            DropIndex("dbo.Attendances", new[] { "ShiftId" });
            DropIndex("dbo.Attendances", new[] { "EmployeeId" });
            DropIndex("dbo.Employees", new[] { "CNIC" });
            DropIndex("dbo.Employees", new[] { "ShiftId" });
            DropIndex("dbo.Employees", new[] { "DesignationId" });
            DropIndex("dbo.Employees", new[] { "DepartmentId" });
            DropIndex("dbo.Employees", new[] { "BranchId" });
            DropIndex("dbo.Employees", new[] { "CompanyId" });
            DropIndex("dbo.Employees", new[] { "EmployeeCode" });
            DropIndex("dbo.Branches", new[] { "BranchCode" });
            DropIndex("dbo.Branches", new[] { "CompanyId" });
            DropIndex("dbo.Vouchers", new[] { "BranchId" });
            DropIndex("dbo.VoucherDetails", new[] { "AccountId" });
            DropIndex("dbo.VoucherDetails", new[] { "VoucherId" });
            DropIndex("dbo.Accounts", new[] { "ParentAccountId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SystemSettings");
            DropTable("dbo.StockTransfers");
            DropTable("dbo.Sewings");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Notifications");
            DropTable("dbo.LeavePolicies");
            DropTable("dbo.Finishings");
            DropTable("dbo.Cuttings");
            DropTable("dbo.SalesOrderDetails");
            DropTable("dbo.Dispatches");
            DropTable("dbo.Invoices");
            DropTable("dbo.Customers");
            DropTable("dbo.SalesOrders");
            DropTable("dbo.ProductionPlans");
            DropTable("dbo.ProductionOrders");
            DropTable("dbo.QualityInspections");
            DropTable("dbo.NCRs");
            DropTable("dbo.CAPAs");
            DropTable("dbo.AuditLogs");
            DropTable("dbo.StockIssues");
            DropTable("dbo.PurchaseOrderDetails");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.GRNs");
            DropTable("dbo.Suppliers");
            DropTable("dbo.FabricBatches");
            DropTable("dbo.Fabrics");
            DropTable("dbo.FabricStocks");
            DropTable("dbo.SalaryStructures");
            DropTable("dbo.SalaryHeads");
            DropTable("dbo.PayrollDetails");
            DropTable("dbo.Payrolls");
            DropTable("dbo.LoanInstallments");
            DropTable("dbo.Loans");
            DropTable("dbo.Leaves");
            DropTable("dbo.EmployeeDocuments");
            DropTable("dbo.Designations");
            DropTable("dbo.Departments");
            DropTable("dbo.Shifts");
            DropTable("dbo.Attendances");
            DropTable("dbo.Employees");
            DropTable("dbo.Companies");
            DropTable("dbo.Branches");
            DropTable("dbo.Vouchers");
            DropTable("dbo.VoucherDetails");
            DropTable("dbo.Accounts");
        }
    }
}
