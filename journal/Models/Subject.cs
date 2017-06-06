using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Subject
    {
        [Key]
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        public Guid? TeacherID { get; set; }
        public virtual User Teacher { get; set; }
        public Guid? SubjectTypeID { get; set; }
        public virtual SubjectType SubjectType { get; set; }       
        public virtual ICollection<Point> Points { get; set; }
        public Subject()
        {
            Points = new List<Point>();
        }
    }
}