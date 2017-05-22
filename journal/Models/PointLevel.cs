﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace journal.Models
{
    public class PointLevel
    {
        [Key]
        [Index(IsUnique = true)]
        
        public Guid ID { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
    }
}