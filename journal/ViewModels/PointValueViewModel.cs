using journal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace journal.ViewModels
{
    public class PointValueViewModel
    {
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid? SchoolID { get; set; }           
        public static explicit operator PointValue(PointValueViewModel model)
        {
            return new PointValue
            {
                Name = model.Name
            };
        }
    }
}