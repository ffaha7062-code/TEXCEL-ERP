namespace TEXCEL_ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Add_BranchId_To_Shift : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "BranchId", c => c.Int(nullable: false, defaultValue: 1));
            // ✅ defaultValue: 1 — pehli branch assign karein
            // Baad mein aap manually update kar sakti hain

            CreateIndex("dbo.Shifts", "BranchId");
            AddForeignKey("dbo.Shifts", "BranchId", "dbo.Branches", "BranchId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Shifts", "BranchId", "dbo.Branches");
            DropIndex("dbo.Shifts", new[] { "BranchId" });
            DropColumn("dbo.Shifts", "BranchId");
        }
    }
}
