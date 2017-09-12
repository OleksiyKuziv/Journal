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
    public class PointLevelViewModels
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "name of point")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "level")]
        public int Level { get; set; }
        public Guid? School { get; set; }
        public List<SelectListItem> Schools { get; set; }
        public string SelectedSchool { get; set; }
        public static explicit operator PointLevel(PointLevelViewModels model)
        {
            return new PointLevel
            {
                Name=model.Name,
                Level=model.Level

            };
        }
    }
}