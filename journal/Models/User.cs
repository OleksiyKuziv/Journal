using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public UserRole UserRollId { get; set; }
        public string email { get; set; }
        public int phone { get; set; }
        public string password { get; set; }
        public Class ClassId { get; set; }
        public string degree { get; set; }
        public string info { get; set; }
    }
}