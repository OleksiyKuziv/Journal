using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.ViewModels
{
    public class ContactUsViewModel
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "first name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name ="last name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name ="description")]
        public string Description { get; set; }
    }
}