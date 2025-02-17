using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyManagement.Models
{
    public class EventReport
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public int PropertyId { get; set; } // Reference to the property associated with the event

        [Required]
        public int ManagerId { get; set; } // Reference to the property manager reporting the event

        [Required]
        public int OwnerId { get; set; } // Reference to the property owner receiving the event report

        [Required]
        public string EventTitle { get; set; } // Title of the event

        [Required]
        public string Description { get; set; } // Detailed description of the event

        [Required]
        public DateTime EventDate { get; set; } // Date when the event occurred

        [Required]
        public DateTime ReportDate { get; set; } // Date when the event was reported

        // Navigation properties
        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }

        [ForeignKey("ManagerId")]
        public virtual PropertyManager PropertyManager { get; set; }

        [ForeignKey("OwnerId")]
        public virtual Owner Owner { get; set; }
    }
}
