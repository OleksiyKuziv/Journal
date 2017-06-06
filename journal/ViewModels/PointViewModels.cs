using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace journal.ViewModels
{
    public class PointViewModels
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "Значення")]
        public Guid? PointValueID { get; set; }
        [Required]
        [Display(Name = "Тип оцінки")]
        public Guid? PointLevelID { get; set; }
        [Required]
        [Display(Name = "Предмет")]
        public Guid? SubjectID { get; set; }
        [Required]
        [Display(Name = "Користувач")]
        public Guid? UserID { get; set; }
        public DateTime Date { get; set; }        
    }
}