using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class TeacherSubject
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SubjectId { get; set; }
    }
}