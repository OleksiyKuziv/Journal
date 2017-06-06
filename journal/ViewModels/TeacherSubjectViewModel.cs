using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace journal.ViewModels
{
    public class TeacherSubjectViewModel
    {
        public List<SelectListItem> Teachers { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        public List<SubjectViewModels> TeacherSubjectViewModels { get; set; }
    }
}