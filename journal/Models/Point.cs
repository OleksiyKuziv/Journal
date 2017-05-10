using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Point
    {
        [Index(IsUnique = true)]
        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid PointLevelId { get; set; }
        public Guid SubjectId { get; set; }
    }
}