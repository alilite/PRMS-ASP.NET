using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PropertyManagement.Models
{
    public class PropertyManagementDbContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PropertyManager> PropertyManagers { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public PropertyManagementDbContext() : base("PropertyManagementDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // For PropertyManagers (related to Owners)
            modelBuilder.Entity<PropertyManager>()
                .HasRequired(pm => pm.Owner)
                .WithMany(o => o.PropertyManagers)
                .HasForeignKey(pm => pm.OwnerId)
                .WillCascadeOnDelete(false);

            // For Properties (related to Owners)
            modelBuilder.Entity<Property>()
                .HasRequired(p => p.Owner)
                .WithMany(o => o.Properties)
                .HasForeignKey(p => p.OwnerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Property>()
                .HasOptional(p => p.Manager)
                .WithMany(m => m.Properties)
                .HasForeignKey(p => p.ManagerId)
                .WillCascadeOnDelete(false);

            // For Apartments (related to Properties and PropertyManagers)
            modelBuilder.Entity<Apartment>()
                .HasRequired(a => a.Property)
                .WithMany(p => p.Apartments)
                .HasForeignKey(a => a.PropertyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Apartment>()
                .HasRequired(a => a.PropertyManager)
                .WithMany(pm => pm.Apartments)
                .HasForeignKey(a => a.ManagerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Appointment>()
                .HasRequired(a => a.Tenant)
                .WithMany(t => t.Appointments)
                .HasForeignKey(a => a.TenantId)
                .WillCascadeOnDelete(false); // Disable cascading delete

            modelBuilder.Entity<Appointment>()
                .HasRequired(a => a.Apartment)
                .WithMany(ap => ap.Appointments)
                .HasForeignKey(a => a.ApartmentId)
                .WillCascadeOnDelete(false); // Disable cascading delete

            modelBuilder.Entity<Appointment>()
                .HasRequired(a => a.PropertyManager)
                .WithMany(pm => pm.Appointments)
                .HasForeignKey(a => a.ManagerId)
                .WillCascadeOnDelete(false); // Disable cascading delete


            // For Messages (Sender and Receiver)
            modelBuilder.Entity<Apartment>()
                   .HasRequired(a => a.Property)
                   .WithMany(p => p.Apartments)
                   .HasForeignKey(a => a.PropertyId)
                   .WillCascadeOnDelete(false);

            // For MaintenanceRequests
            modelBuilder.Entity<MaintenanceRequest>()
                .HasRequired(mr => mr.Apartment)
                .WithMany(ap => ap.MaintenanceRequests)
                .HasForeignKey(mr => mr.ApartmentId)
                .WillCascadeOnDelete(false);

            // For Payments
            modelBuilder.Entity<Payment>()
                .HasRequired(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .HasRequired(p => p.Apartment)
                .WithMany()
                .HasForeignKey(p => p.ApartmentId)
                .WillCascadeOnDelete(false);

            // Configure relationships for EventReport
            modelBuilder.Entity<EventReport>()
                .HasRequired(er => er.Property)
                .WithMany(p => p.EventReports)
                .HasForeignKey(er => er.PropertyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EventReport>()
                .HasRequired(er => er.PropertyManager)
                .WithMany(pm => pm.EventReports)
                .HasForeignKey(er => er.ManagerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EventReport>()
                .HasRequired(er => er.Owner)
                .WithMany(o => o.EventReports)
                .HasForeignKey(er => er.OwnerId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

    }
}