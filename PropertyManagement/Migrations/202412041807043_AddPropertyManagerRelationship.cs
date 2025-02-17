namespace PropertyManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyManagerRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Properties", "ManagerId", c => c.Int());
            CreateIndex("dbo.Properties", "ManagerId");
            AddForeignKey("dbo.Properties", "ManagerId", "dbo.PropertyManagers", "ManagerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Properties", "ManagerId", "dbo.PropertyManagers");
            DropIndex("dbo.Properties", new[] { "ManagerId" });
            DropColumn("dbo.Properties", "ManagerId");
        }
    }
}
