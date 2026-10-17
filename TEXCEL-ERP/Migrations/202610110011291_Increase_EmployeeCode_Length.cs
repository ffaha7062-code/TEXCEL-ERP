namespace TEXCEL_ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Increase_EmployeeCode_Length : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Employees", new[] { "EmployeeCode" });
            AlterColumn("dbo.Employees", "EmployeeCode", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Employees", "EmployeeCode", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Employees", new[] { "EmployeeCode" });
            AlterColumn("dbo.Employees", "EmployeeCode", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Employees", "EmployeeCode", unique: true);
        }
    }
}
