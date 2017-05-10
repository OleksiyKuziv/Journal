using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Rating
    {
        [Index(IsUnique = true)]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PointId { get; set; }
    }
}