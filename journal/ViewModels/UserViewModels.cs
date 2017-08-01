using journal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.ViewModels
{
    public class UserViewModels
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "first name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "last name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "age")]
        public int Age { get; set; }
        public Guid? UserRollID { get; set; }
        [Required]
        [Display(Name = "email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "phone number")]
        public string Phone { get; set; }
        public string Password { get; set; }
        public Guid? ClassID { get; set; }
        public string Degree { get; set; }
        public string Info { get; set; }
        public string UserRollSelected { get; set; }
        public string ClassSelected { get; set; }
        public string RegisterDate { get; set; }
        public Guid? SchoolID { get; set; }
        public string SelectedSchool {get;set;}
        public static explicit operator User(UserViewModels model) {
            return new User
            {
                ID = model.ID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                UserRollID=model.UserRollID,
                Email=model.Email,
                Phone=model.Phone,
                Password=model.Password,
                ClassID=model.ClassID,
                Degree=model.Degree,
                Info=model.Info,
                RegisterDate=model.RegisterDate,
                SchoolID=model.SchoolID
            };
        }
    }
}