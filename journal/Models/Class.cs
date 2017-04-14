using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace journal.Models
{
     public  class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public User UserId { get; set; }
    }
}
