using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.ViewModels
{
    public class MessageViewModels
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "from user")]
        public Guid? FromUserID { get; set; }
        [Required]
        [Display(Name = "to user")]
        public Guid? ToUserID { get; set; }
        [Required]
        [Display(Name = "type of message")]
        public Guid? MessageTypeID { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        [Display(Name = "subject")]
        public string Subject { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}