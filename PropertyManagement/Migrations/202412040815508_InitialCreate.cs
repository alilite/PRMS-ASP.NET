namespace PropertyManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Apartments",
                c => new
                    {
                        ApartmentId = c.Int(nullable: false, identity: true),
                        PropertyId = c.Int(nullable: false),
                        ManagerId = c.Int(nullable: false),
                        Number = c.String(),
                        RentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AvailabilityStatus = c.String(),
                        Bathroom = c.Int(nullable: false),
                        Bedroom = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApartmentId)
                .ForeignKey("dbo.Properties", t => t.PropertyId)
                .ForeignKey("dbo.PropertyManagers", t => t.ManagerId)
                .Index(t => t.PropertyId)
                .Index(t => t.ManagerId);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        ManagerId = c.Int(nullable: false),
                        ApartmentId = c.Int(nullable: false),
                        AppointmentDate = c.DateTime(nullable: false),
                        AppointmentTime = c.Time(nullable: false, precision: 7),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Apartments", t => t.ApartmentId)
                .ForeignKey("dbo.PropertyManagers", t => t.ManagerId)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .Index(t => t.TenantId)
                .Index(t => t.ManagerId)
                .Index(t => t.ApartmentId);
            
            CreateTable(
                "dbo.PropertyManagers",
                c => new
                    {
                        ManagerId = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ManagerId)
                .ForeignKey("dbo.Owners", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        OwnerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.OwnerId);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        PropertyId = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        Name = c.String(),
                        Location = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.PropertyId)
                .ForeignKey("dbo.Owners", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        TenantId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.TenantId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        ApartmentId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        Tenant_TenantId = c.Int(),
                        Apartment_ApartmentId = c.Int(),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Apartments", t => t.ApartmentId)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .ForeignKey("dbo.Tenants", t => t.Tenant_TenantId)
                .ForeignKey("dbo.Apartments", t => t.Apartment_ApartmentId)
                .Index(t => t.TenantId)
                .Index(t => t.ApartmentId)
                .Index(t => t.Tenant_TenantId)
                .Index(t => t.Apartment_ApartmentId);
            
            CreateTable(
                "dbo.MaintenanceRequests",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        ApartmentId = c.Int(nullable: false),
                        ReportedById = c.Int(nullable: false),
                        Description = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.Apartments", t => t.ApartmentId)
                .Index(t => t.ApartmentId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        Content = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.PropertyManagers", t => t.ReceiverId)
                .ForeignKey("dbo.Tenants", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "SenderId", "dbo.Tenants");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.PropertyManagers");
            DropForeignKey("dbo.Apartments", "ManagerId", "dbo.PropertyManagers");
            DropForeignKey("dbo.Apartments", "PropertyId", "dbo.Properties");
            DropForeignKey("dbo.Payments", "Apartment_ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.MaintenanceRequests", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.Appointments", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Payments", "Tenant_TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Payments", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Payments", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.Appointments", "ManagerId", "dbo.PropertyManagers");
            DropForeignKey("dbo.PropertyManagers", "OwnerId", "dbo.Owners");
            DropForeignKey("dbo.Properties", "OwnerId", "dbo.Owners");
            DropForeignKey("dbo.Appointments", "ApartmentId", "dbo.Apartments");
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.MaintenanceRequests", new[] { "ApartmentId" });
            DropIndex("dbo.Payments", new[] { "Apartment_ApartmentId" });
            DropIndex("dbo.Payments", new[] { "Tenant_TenantId" });
            DropIndex("dbo.Payments", new[] { "ApartmentId" });
            DropIndex("dbo.Payments", new[] { "TenantId" });
            DropIndex("dbo.Properties", new[] { "OwnerId" });
            DropIndex("dbo.PropertyManagers", new[] { "OwnerId" });
            DropIndex("dbo.Appointments", new[] { "ApartmentId" });
            DropIndex("dbo.Appointments", new[] { "ManagerId" });
            DropIndex("dbo.Appointments", new[] { "TenantId" });
            DropIndex("dbo.Apartments", new[] { "ManagerId" });
            DropIndex("dbo.Apartments", new[] { "PropertyId" });
            DropTable("dbo.Messages");
            DropTable("dbo.MaintenanceRequests");
            DropTable("dbo.Payments");
            DropTable("dbo.Tenants");
            DropTable("dbo.Properties");
            DropTable("dbo.Owners");
            DropTable("dbo.PropertyManagers");
            DropTable("dbo.Appointments");
            DropTable("dbo.Apartments");
        }
    }
}
