using journal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace journal.ViewModels
{
    public class ClassViewModels
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        [Required]
        [Display(Name ="name of class")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "year of studing")]
        public int Year { get; set; }
        public Guid? SchoolID { get; set; }       
        public List<SelectListItem> Schools { get; set; }
        public string SelectedSchool { get; set; }
        public Guid? UserID { get; set; }
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> NewUsersList { get; set; }
        public string SelectedNewPupil { get; set; }
        public static explicit operator Class(ClassViewModels model)
        {
            return new Class
            {
                Name=model.Name,
                Year=model.Year,
                SchoolID=Guid.Parse(model.SelectedSchool)
            };
        }
    }
}