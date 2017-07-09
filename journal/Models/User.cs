using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class User
    {
        [Key]
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Guid? UserRollID { get; set; }
        public virtual UserRole UserRole { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public Guid? ClassID { get; set; }
        public virtual Class Class { get; set; }
        public string Degree { get; set; }
        public string Info { get; set; }
        public DateTime RegisterDate { get; set; }
        public Guid? SchoolID { get; set; }
        public virtual School School { get; set; }
        public ICollection<StudySubject> StudySubjects { get; set; }
        public ICollection<Point> Points { get; set; }
        public User()
        {
            StudySubjects = new List<StudySubject>();
            Points = new List<Point>();
        }
    }
}