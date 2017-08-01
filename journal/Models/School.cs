using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class School
    {
        [Key]
        [Index(IsUnique = true)]
        public Guid ID { get; set; }       
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string TypeSchool { get; set; }
        public string Degree { get; set; }
        public string OwnerShip { get; set; }
        public int ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Regulatory { get; set; }
        public string TimeStamp { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public School()
        {
            Users = new List<User>();
            Classes = new List<Class>();

        }
    }
}