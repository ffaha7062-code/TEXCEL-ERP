namespace TEXCEL_ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AttendanceFields_And_Holiday : DbMigration
    {
        public override void Up()
        {
            // ✅ Existing records ke liye defaultValue: 1
            AddColumn("dbo.Attendances", "BranchId", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.Attendances", "LessTimeHours", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0m));
            AddColumn("dbo.Attendances", "IsVerified", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Attendances", "VerifiedBy", c => c.Int());
            AddColumn("dbo.Attendances", "VerifiedDate", c => c.DateTime());
            AddColumn("dbo.Attendances", "ModifiedDate", c => c.DateTime());

            CreateIndex("dbo.Attendances", "BranchId");
            AddForeignKey("dbo.Attendances", "BranchId", "dbo.Branches", "BranchId");

            CreateTable(
                "dbo.Holidays",
                c => new
                {
                    HolidayId = c.Int(nullable: false, identity: true),
                    BranchId = c.Int(nullable: false),
                    HolidayName = c.String(nullable: false, maxLength: 100),
                    HolidayDate = c.DateTime(nullable: false),
                    Description = c.String(maxLength: 500),
                    IsPaid = c.Boolean(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.HolidayId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.BranchId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Holidays", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Attendances", "BranchId", "dbo.Branches");
            DropIndex("dbo.Holidays", new[] { "BranchId" });
            DropIndex("dbo.Attendances", new[] { "BranchId" });
            DropColumn("dbo.Attendances", "ModifiedDate");
            DropColumn("dbo.Attendances", "VerifiedDate");
            DropColumn("dbo.Attendances", "VerifiedBy");
            DropColumn("dbo.Attendances", "IsVerified");
            DropColumn("dbo.Attendances", "LessTimeHours");
            DropColumn("dbo.Attendances", "BranchId");
            DropTable("dbo.Holidays");
        }
    }
}
