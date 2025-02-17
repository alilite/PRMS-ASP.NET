using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PropertyManagement.Models
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }
        public int OwnerId { get; set; }
        public int? ManagerId { get; set; } // Nullable, as some properties might not have a manager
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual PropertyManager Manager { get; set; }
        public virtual ICollection<Apartment> Apartments { get; set; }
        public virtual ICollection<EventReport> EventReports { get; set; }

    }
}