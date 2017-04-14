using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Message
    {
        public int Id { get; set; }
        public User FromUserId { get; set; }
        public User ToUserId { get; set; }
        public MessageType MessageTypeId { get; set; }
        public string Text { get; set; }
        public string Subject { get; set; }
    }
}