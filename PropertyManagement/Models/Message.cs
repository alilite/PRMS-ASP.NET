using System.ComponentModel.DataAnnotations;
using System;

namespace PropertyManagement.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string SenderRole { get; set; }
        public string ReceiverRole { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
