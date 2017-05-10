using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Message
    {
        [Index(IsUnique = true)]
        public Guid Id { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public Guid MessageTypeId { get; set; }
        public string Text { get; set; }
        public string Subject { get; set; }
    }
}