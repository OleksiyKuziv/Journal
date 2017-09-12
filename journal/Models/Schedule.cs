using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Schedule
    {
        [Key]
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        public Guid? SubjectID { get; set; }        
        public string WeklyStartTime { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public Schedule()
        {
            Subjects = new List<Subject>();
        }
    }
}