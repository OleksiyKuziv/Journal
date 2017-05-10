using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class User
    {
        [Index(IsUnique = true)]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Guid UserRollId { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public string Password { get; set; }
        public Guid ClassId { get; set; }
        public string Degree { get; set; }
        public string Info { get; set; }
    }
}