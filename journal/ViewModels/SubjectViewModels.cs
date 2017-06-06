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
    public class SubjectViewModels
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }        
        public Guid? TeacherID { get; set; }
        public List<SelectListItem> Teachers { get; set; }
        [Required]
        [Display(Name = "teacher")]
        public string SelectedTeacher { get; set; }
        public Guid? SubjectTypeID { get; set; }
        public List<SelectListItem> SubjectTypes { get; set; }
        [Required]
        [Display(Name = "subject")]
        public string SelectedSubjectType { get; set; }
        public static explicit operator Subject (SubjectViewModels model)
        {
            return new Subject
            {
                ID=model.ID,
                TeacherID = Guid.Parse(model.SelectedTeacher),
                SubjectTypeID=Guid.Parse(model.SelectedSubjectType)
            };
        }
         
    }
}