using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace journal.ViewModels
{
    public class PointViewModels
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "Значення")]
        public Guid? PointValueID { get; set; }
        public string SelectedPointValue { get; set; }
        public List<SelectListViewModel> PointValues { get; set; }
        [Required]
        [Display(Name = "Тип оцінки")]
        public Guid? PointLevelID { get; set; }
        public string SelectedPointLevel { get; set; }
        public List<SelectListViewModel> PointLevels { get; set; }
        [Required]
        [Display(Name = "Предмет")]
        public Guid? SubjectID { get; set; }
        public string SelectedSubject { get; set; }
        [Required]
        [Display(Name = "Користувач")]
        public Guid? UserID { get; set; }
        public List<SelectListViewModel> Users { get; set; }
        public string SelectedUser { get; set; }
        public DateTime Date { get; set; } 
        public string StrDate { get; set; }
    }
}