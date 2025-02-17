using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyManagement.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PropertyManager
    {
        [Key]
        public int ManagerId { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual ICollection<Apartment> Apartments { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }

        // Add this property for the relationship with Properties
        public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<EventReport> EventReports { get; set; }

    }

}