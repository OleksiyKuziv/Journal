using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace journal.ViewModels
{
    public class TeacherPutPointViewModel
    {
        public List<SelectListViewModel> Users { get; set; }
        public List<SelectListViewModel> PointValues { get; set; }
        public List<SelectListViewModel> PointLevels { get; set; }
        public List<SelectListViewModel> Subjects { get; set; }
        public List<PointViewModels> PointsView { get; set; }
        public List<SelectListViewModel> Classes { get; set; }
    }
}