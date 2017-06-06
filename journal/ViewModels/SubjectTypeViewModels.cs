using journal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.ViewModels
{
    public class SubjectTypeViewModels
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "name of subject")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "description")]
        public string Description { get; set; }
        public static explicit operator SubjectType(SubjectTypeViewModels model)
        {
            return new SubjectType
            {
                Name = model.Name,
                Description=model.Description
            };
        } 
    }
}