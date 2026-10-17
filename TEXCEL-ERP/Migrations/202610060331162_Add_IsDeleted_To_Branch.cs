namespace TEXCEL_ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsDeleted_To_Branch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Branches", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Branches", "IsDeleted");
        }
    }
}
