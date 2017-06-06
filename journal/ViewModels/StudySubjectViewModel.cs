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
    public class StudySubjectViewModel
    {
        [Key]
        [Index(IsUnique = true)]
        public Guid ID { get; set; }           
        public Guid? UserID { get; set; }
        public List<SelectListItem> Users { get; set; }
        [Required]
        [Display(Name = "student")]
        public string SelectedUser { get; set; }
        public Guid? SubjectID { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        [Required]
        [Display(Name = "subject")]
        public string SelectedSubject {get;set;}
        public bool IsActive { get; set; }
        public static explicit operator StudySubject(StudySubjectViewModel model)
        {
            return new StudySubject
            {
                UserID=Guid.Parse(model.SelectedUser),
                SubjectID=Guid.Parse(model.SelectedSubject)
            };
        }
       
    }
}