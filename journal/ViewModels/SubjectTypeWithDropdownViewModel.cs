using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace journal.ViewModels
{
    public class SubjectTypeWithDropdownViewModel
    {
        public List<SelectListItem> Schools { get; set; }
        public List<SubjectTypeViewModels> SubjectTypes { get; set; }
    }
}