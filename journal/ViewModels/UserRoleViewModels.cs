using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.ViewModels
{
    public class UserRoleViewModels
    {
        [Index(IsUnique = true)]
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "name of role")]
        public string Name { get; set; }
    }
}