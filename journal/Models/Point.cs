using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Point
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public Guid PointLevelId { get; set; }
        public Guid SubjectId { get; set; }
    }
}