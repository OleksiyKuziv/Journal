using journal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.ViewModels
{
    public class MessageTypeViewModels
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "type of message")]
        public string Name { get; set; }
        public static explicit operator MessageType(MessageTypeViewModels model)
        {
            return new MessageType
            {
                Name = model.Name
            };
        }
    }
}