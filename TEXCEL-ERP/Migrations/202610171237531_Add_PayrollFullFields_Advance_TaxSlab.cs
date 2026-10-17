namespace TEXCEL_ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PayrollFullFields_Advance_TaxSlab : DbMigration
    {
        public override void Up()
        {
            // Existing Payrolls table — add columns with defaults
            AddColumn("dbo.Payrolls", "BranchId", c => c.Int());
            AddColumn("dbo.Payrolls", "DepartmentId", c => c.Int());
            AddColumn("dbo.Payrolls", "GrossSalary", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m));
            AddColumn("dbo.Payrolls", "TotalWorkingDays", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Payrolls", "HalfDays", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Payrolls", "Holidays", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Payrolls", "TotalWorkedHours", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m));
            AddColumn("dbo.Payrolls", "LateMinutes", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Payrolls", "LateDeduction", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m));
            AddColumn("dbo.Payrolls", "LessHours", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m));
            AddColumn("dbo.Payrolls", "LessTimeDeduction", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m));
            AddColumn("dbo.Payrolls", "AbsentDeduction", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m));
            AddColumn("dbo.Payrolls", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.Payrolls", "ApprovedById", c => c.Int());
            AddColumn("dbo.Payrolls", "PaidDate", c => c.DateTime());
            AddColumn("dbo.Payrolls", "PaymentMethod", c => c.String(maxLength: 20));
            AddColumn("dbo.Payrolls", "Remarks", c => c.String(maxLength: 500));
            AddColumn("dbo.Payrolls", "CreatedById", c => c.Int());
            AddColumn("dbo.Payrolls", "ModifiedDate", c => c.DateTime());
            CreateIndex("dbo.Payrolls", "BranchId");
            CreateIndex("dbo.Payrolls", "DepartmentId");

            // SalaryHeads — new fields
            AddColumn("dbo.SalaryHeads", "IsSystemHead", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.SalaryHeads", "IsStatutory", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.SalaryHeads", "IsDeduction", c => c.Boolean(nullable: false, defaultValue: false));

            // ✅ NAYI TABLES
            CreateTable(
                "dbo.Advances",
                c => new
                {
                    AdvanceId = c.Int(nullable: false, identity: true),
                    EmployeeId = c.Int(nullable: false),
                    AdvanceAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    RecoveredAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    OutstandingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Installments = c.Int(nullable: false),
                    MonthlyInstallment = c.Decimal(nullable: false, precision: 18, scale: 2),
                    AdvanceDate = c.DateTime(nullable: false),
                    StartDeductionDate = c.DateTime(),
                    Reason = c.String(maxLength: 500),
                    Status = c.Int(nullable: false),
                    ApprovedById = c.Int(),
                    ApprovedDate = c.DateTime(),
                    DisbursedById = c.Int(),
                    DisbursedDate = c.DateTime(),
                    PaymentMethod = c.String(maxLength: 20),
                    Remarks = c.String(maxLength: 500),
                    CreatedDate = c.DateTime(nullable: false),
                    ModifiedDate = c.DateTime(),
                })
                .PrimaryKey(t => t.AdvanceId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);

            CreateTable(
                "dbo.AdvanceRecoveries",
                c => new
                {
                    AdvanceRecoveryId = c.Int(nullable: false, identity: true),
                    AdvanceId = c.Int(nullable: false),
                    PayrollId = c.Int(),
                    RecoveryDate = c.DateTime(nullable: false),
                    Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Remarks = c.String(maxLength: 500),
                    CreatedDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.AdvanceRecoveryId)
                .ForeignKey("dbo.Advances", t => t.AdvanceId)
                .ForeignKey("dbo.Payrolls", t => t.PayrollId)
                .Index(t => t.AdvanceId)
                .Index(t => t.PayrollId);

            CreateTable(
                "dbo.TaxSlabs",
                c => new
                {
                    TaxSlabId = c.Int(nullable: false, identity: true),
                    Year = c.Int(nullable: false),
                    MinIncome = c.Decimal(nullable: false, precision: 18, scale: 2),
                    MaxIncome = c.Decimal(nullable: false, precision: 18, scale: 2),
                    FixedTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Description = c.String(maxLength: 200),
                    IsActive = c.Boolean(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.TaxSlabId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.AdvanceRecoveries", "PayrollId", "dbo.Payrolls");
            DropForeignKey("dbo.AdvanceRecoveries", "AdvanceId", "dbo.Advances");
            DropForeignKey("dbo.Advances", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.AdvanceRecoveries", new[] { "PayrollId" });
            DropIndex("dbo.AdvanceRecoveries", new[] { "AdvanceId" });
            DropIndex("dbo.Advances", new[] { "EmployeeId" });
            DropIndex("dbo.Payrolls", new[] { "DepartmentId" });
            DropIndex("dbo.Payrolls", new[] { "BranchId" });
            DropTable("dbo.TaxSlabs");
            DropTable("dbo.AdvanceRecoveries");
            DropTable("dbo.Advances");
            DropColumn("dbo.SalaryHeads", "IsDeduction");
            DropColumn("dbo.SalaryHeads", "IsStatutory");
            DropColumn("dbo.SalaryHeads", "IsSystemHead");
            DropColumn("dbo.Payrolls", "ModifiedDate");
            DropColumn("dbo.Payrolls", "CreatedById");
            DropColumn("dbo.Payrolls", "Remarks");
            DropColumn("dbo.Payrolls", "PaymentMethod");
            DropColumn("dbo.Payrolls", "PaidDate");
            DropColumn("dbo.Payrolls", "ApprovedById");
            DropColumn("dbo.Payrolls", "ApprovedDate");
            DropColumn("dbo.Payrolls", "AbsentDeduction");
            DropColumn("dbo.Payrolls", "LessTimeDeduction");
            DropColumn("dbo.Payrolls", "LessHours");
            DropColumn("dbo.Payrolls", "LateDeduction");
            DropColumn("dbo.Payrolls", "LateMinutes");
            DropColumn("dbo.Payrolls", "TotalWorkedHours");
            DropColumn("dbo.Payrolls", "Holidays");
            DropColumn("dbo.Payrolls", "HalfDays");
            DropColumn("dbo.Payrolls", "TotalWorkingDays");
            DropColumn("dbo.Payrolls", "GrossSalary");
            DropColumn("dbo.Payrolls", "DepartmentId");
            DropColumn("dbo.Payrolls", "BranchId");
        }
    }
}
