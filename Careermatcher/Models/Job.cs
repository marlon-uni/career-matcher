﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class Job
    {
        [Key,Column(Order=1)]
        public string EmployerEmailAddress { get; set; }
        [Key, Column(Order = 2)]
        public string JobTitle { get; set; }
        [Key, Column(Order = 3)]
        public DateTime PublishDate { get; set; }
        public string Education { get; set; }
        public string Tags { get; set; }
    }

    public class JobDBContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
    }
}
