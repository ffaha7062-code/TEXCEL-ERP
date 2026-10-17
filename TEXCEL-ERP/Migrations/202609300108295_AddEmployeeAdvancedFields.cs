namespace TEXCEL_ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeAdvancedFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            AddColumn("dbo.Employees", "SkillType", c => c.Int());
            AddColumn("dbo.Employees", "MachineAssigned", c => c.String(maxLength: 100));
            AddColumn("dbo.Employees", "ReportingManagerId", c => c.Int());
            AddColumn("dbo.Employees", "PieceRate", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Employees", "CNICFrontPath", c => c.String(maxLength: 500));
            AddColumn("dbo.Employees", "CNICBackPath", c => c.String(maxLength: 500));
            AlterColumn("dbo.Employees", "Nationality", c => c.String(maxLength: 50));
            CreateIndex("dbo.Employees", "ReportingManagerId");
            AddForeignKey("dbo.Employees", "ReportingManagerId", "dbo.Employees", "EmployeeId");
            AddForeignKey("dbo.Employees", "CompanyId", "dbo.Companies", "CompanyId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Employees", "ReportingManagerId", "dbo.Employees");
            DropIndex("dbo.Employees", new[] { "ReportingManagerId" });
            AlterColumn("dbo.Employees", "Nationality", c => c.String(maxLength: 100));
            DropColumn("dbo.Employees", "CNICBackPath");
            DropColumn("dbo.Employees", "CNICFrontPath");
            DropColumn("dbo.Employees", "PieceRate");
            DropColumn("dbo.Employees", "ReportingManagerId");
            DropColumn("dbo.Employees", "MachineAssigned");
            DropColumn("dbo.Employees", "SkillType");
            AddForeignKey("dbo.Employees", "CompanyId", "dbo.Companies", "CompanyId");
        }
    }
}
