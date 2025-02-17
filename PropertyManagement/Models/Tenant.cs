using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PropertyManagement.Models
{
    public class Tenant
    {
        [Key] // Define primary key
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}