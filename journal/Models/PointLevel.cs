using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class PointLevel
    {
        [Index(IsUnique = true)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
    }
}