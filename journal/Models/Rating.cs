using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PointId { get; set; }
    }
}