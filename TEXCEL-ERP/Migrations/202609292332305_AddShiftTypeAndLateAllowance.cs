namespace TEXCEL_ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShiftTypeAndLateAllowance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "ShiftType", c => c.String(maxLength: 20));
            DropColumn("dbo.Shifts", "OvertimeStartAfter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shifts", "OvertimeStartAfter", c => c.Int(nullable: false));
            DropColumn("dbo.Shifts", "ShiftType");
        }
    }
}
