using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Point
    {
        [Key]
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        public Guid? PointValueID { get; set; }
        public virtual PointValue PointValue { get; set; }
        public Guid? PointLevelID { get; set; }
        public virtual PointLevel PointLevel { get; set; }
        public Guid? SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime Date { get; set; }        
    }
}