using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace journal.Models
{
   public class SubjectType
    {
        [Key]
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
