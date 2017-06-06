using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace journal.Models
{
    public class Class
    {
        [Key]
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public Guid? SchoolID { get; set; }
        public virtual School School { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public Class()
        {
            Users = new List<User>();
        }
    }
}
