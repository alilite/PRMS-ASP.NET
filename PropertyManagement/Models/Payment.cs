using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PropertyManagement.Models
{
    public class Payment
    {
        [Key] // Define primary key
        public int PaymentId { get; set; }
        public int TenantId { get; set; }
        public int ApartmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual Apartment Apartment { get; set; }
    }
}