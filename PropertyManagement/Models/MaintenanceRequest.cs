using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PropertyManagement.Models
{
    public class MaintenanceRequest
    {
        [Key] // This explicitly sets RequestId as the primary key
        public int RequestId { get; set; }

        public int ApartmentId { get; set; }
        public int ReportedById { get; set; } // TenantId or ManagerId
        public string Description { get; set; }
        public string Status { get; set; } // e.g., Pending, In Progress, Completed

        public virtual Apartment Apartment { get; set; }
    }
}