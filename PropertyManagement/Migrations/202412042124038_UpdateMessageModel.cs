namespace PropertyManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMessageModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.PropertyManagers");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.Tenants");
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            AddColumn("dbo.Messages", "SenderRole", c => c.String());
            AddColumn("dbo.Messages", "ReceiverRole", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "ReceiverRole");
            DropColumn("dbo.Messages", "SenderRole");
            CreateIndex("dbo.Messages", "ReceiverId");
            CreateIndex("dbo.Messages", "SenderId");
            AddForeignKey("dbo.Messages", "SenderId", "dbo.Tenants", "TenantId");
            AddForeignKey("dbo.Messages", "ReceiverId", "dbo.PropertyManagers", "ManagerId");
        }
    }
}
