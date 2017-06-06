using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Message
    {
        [Key]
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        public Guid? FromUserID { get; set; }
        public virtual User FromUser { get; set; }
        public Guid? ToUserID { get; set; }
        public virtual User ToUser { get; set; }
        public Guid? MessageTypeID { get; set; }
        public virtual MessageType MessageType { get; set; }
        public string Text { get; set; }
        public string Subject { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}