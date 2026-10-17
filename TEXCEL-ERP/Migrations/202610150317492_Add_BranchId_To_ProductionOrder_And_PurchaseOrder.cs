namespace TEXCEL_ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Add_BranchId_To_ProductionOrder_And_PurchaseOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductionOrders", "BranchId", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.ProductionOrders", "BranchId");
            AddForeignKey("dbo.ProductionOrders", "BranchId", "dbo.Branches", "BranchId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.ProductionOrders", "BranchId", "dbo.Branches");
            DropIndex("dbo.ProductionOrders", new[] { "BranchId" });
            DropColumn("dbo.ProductionOrders", "BranchId");
        }
    }
}