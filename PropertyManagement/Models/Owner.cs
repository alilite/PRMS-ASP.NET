using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PropertyManagement.Models
{
    public class Owner
    {
        [Key] // Define primary key
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public virtual ICollection<PropertyManager> PropertyManagers { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<EventReport> EventReports { get; set; }

    }
}