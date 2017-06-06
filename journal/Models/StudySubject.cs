using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class StudySubject
    {
        [Key]
        [Index(IsUnique =true)]
        public Guid ID { get; set; }
        public Guid? UserID { get; set; }
        public virtual User User { get; set; }
        public Guid? SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
    }
}