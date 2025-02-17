using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PropertyManagement.Models
{
    public class Apartment
    {
        [Key] // Define primary key
        public int ApartmentId { get; set; }
        public int PropertyId { get; set; }
        public int ManagerId { get; set; }
        public string Number { get; set; }
        public decimal RentAmount { get; set; }
        public string AvailabilityStatus { get; set; }
        public int Bathroom { get; set; }
        public int Bedroom { get; set; }

        public virtual Property Property { get; set; }
        public virtual PropertyManager PropertyManager { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}