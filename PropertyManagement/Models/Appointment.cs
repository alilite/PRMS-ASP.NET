using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PropertyManagement.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int TenantId { get; set; }
        public int ManagerId { get; set; }
        public int ApartmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Status { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual Apartment Apartment { get; set; }
        public virtual PropertyManager PropertyManager { get; set; } // Add this navigation property
    }
}