using journal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace journal.ViewModels
{
    public class SchoolViewModel
    {
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "full name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "short name")]
        public string ShortName { get; set; }
        [Required]
        [Display(Name = "type of school")]
        public string TypeSchool { get; set; }
        [Required]
        [Display(Name = "degree")]
        public string Degree { get; set; }
        [Required]
        [Display(Name = "owner ship")]
        public string OwnerShip { get; set; }
        [Required]
        [Display(Name = "zip code")]
        public int ZipCode { get; set; }
        [Required]
        [Display(Name = "address")]
        public string Address1 { get; set; }
        [Required]
        [Display(Name = "address")]
        public string Address2 { get; set; }
        [Required]
        [Display(Name = "phone number")]
        public long PhoneNumber { get; set; }
        [Required]
        [Display(Name = "email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "regulatory")]
        public int Regulatory { get; set; }
        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public static explicit operator School(SchoolViewModel model)
        {
            return new School
            {
                ID=model.ID,
                FullName=model.FullName,
                ShortName=model.ShortName,
                TypeSchool=model.TypeSchool,
                Degree=model.Degree,
                OwnerShip=model.OwnerShip,
                ZipCode=model.ZipCode,
                Address1=model.Address1,
                Address2=model.Address2,
                PhoneNumber=model.PhoneNumber,
                Email=model.Email,
                Regulatory=model.Regulatory                
            };
        }
    }
}