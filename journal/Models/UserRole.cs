using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace journal.Models
{
    public class UserRole
    {
        [Index(IsUnique = true)]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
