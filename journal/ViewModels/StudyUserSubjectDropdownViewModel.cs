using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace journal.ViewModels
{
    public class StudyUserSubjectDropdownViewModel
    {
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        public List<StudySubjectViewModel> StudySubjects { get; set; }
    }
}