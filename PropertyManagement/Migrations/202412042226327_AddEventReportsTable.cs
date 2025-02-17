namespace PropertyManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventReportsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventReports",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        PropertyId = c.Int(nullable: false),
                        ManagerId = c.Int(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        EventTitle = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        EventDate = c.DateTime(nullable: false),
                        ReportDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Owners", t => t.OwnerId)
                .ForeignKey("dbo.Properties", t => t.PropertyId)
                .ForeignKey("dbo.PropertyManagers", t => t.ManagerId)
                .Index(t => t.PropertyId)
                .Index(t => t.ManagerId)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventReports", "ManagerId", "dbo.PropertyManagers");
            DropForeignKey("dbo.EventReports", "PropertyId", "dbo.Properties");
            DropForeignKey("dbo.EventReports", "OwnerId", "dbo.Owners");
            DropIndex("dbo.EventReports", new[] { "OwnerId" });
            DropIndex("dbo.EventReports", new[] { "ManagerId" });
            DropIndex("dbo.EventReports", new[] { "PropertyId" });
            DropTable("dbo.EventReports");
        }
    }
}
