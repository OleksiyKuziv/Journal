using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Message
    {
        public int Id { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public Guid MessageTypeId { get; set; }
        public string Text { get; set; }
        public string Subject { get; set; }
    }
}