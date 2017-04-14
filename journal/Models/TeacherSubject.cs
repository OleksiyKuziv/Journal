using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class TeacherSubject
    {
        public int Id { get; set; }
        public User UserId { get; set; }
        public Subject SubjectId { get; set; }
    }
}