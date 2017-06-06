using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class PointValue
    {
        [Key]
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid? SchoolID { get; set; }
        public virtual School School { get; set; }

    }
}